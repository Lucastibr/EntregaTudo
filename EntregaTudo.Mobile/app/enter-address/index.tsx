import React, { useState, useEffect } from 'react';
import { View, Text, TextInput, TouchableOpacity, StyleSheet, Alert, ImageBackground } from 'react-native';
import { useNavigation, useRoute } from '@react-navigation/native';
import axios from 'axios';
import logo from '../../assets/images/logo.jpg';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { BASE_URL, API_KEY } from '@/config';
import * as Location from 'expo-location';
import Modal from 'react-native-modal';

export default function EnterAddressScreen() {
  const navigation = useNavigation();
  const route = useRoute();
  const { item } = route.params as { item: string };

  const [cep, setCep] = useState('');
  const [address, setAddress] = useState({
    street: '',
    neighborhood: '',
    city: '',
    state: '',
    postalCode: '',
  });
  const [number, setNumber] = useState('');
  const [complement, setComplement] = useState('');
  const [originAddress, setOriginAddress] = useState({
    street: '',
    neighborhood: '',
    city: '',
    state: '',
    postalCode: '',
    latitude: 0,
    longitude: 0,
  });
  const [isModalVisible, setModalVisible] = useState(false);
  const [modalMessage, setModalMessage] = useState('');
  const [deliveryCost, setDeliveryCost] = useState<number | null>(null);

  useEffect(() => {
    const getCurrentLocation = async () => {
      let { status } = await Location.requestForegroundPermissionsAsync();
      if (status !== 'granted') {
        showModal('Permissão negada', 'Não foi possível obter a localização do dispositivo.');
        return;
      }

      let location = await Location.getCurrentPositionAsync({});
      const { latitude, longitude } = location.coords;

      try {
        const response = await axios.get('https://api.opencagedata.com/geocode/v1/json', {
          params: {
            key: `${API_KEY}`,
            q: `${latitude},${longitude}`,
            language: 'pt-br',
          },
        });

        const { results } = response.data;
        if (results.length > 0) {
          const locationData = results[0].components;

          setOriginAddress({
            street: locationData.road || '',
            neighborhood: locationData.suburb || '',
            city: locationData.city || locationData.town || locationData.village || '',
            state: locationData.state || '',
            postalCode: locationData.postcode || '',
            latitude,
            longitude,
          });
        } else {
          showModal('Erro', 'Não foi possível encontrar o endereço com base na localização.');
        }
      } catch (error) {
        console.error('Erro ao buscar endereço:', error);
        showModal('Erro', 'Não foi possível buscar o endereço com base na localização.');
      }
    };

    getCurrentLocation();
  }, []);

  const showModal = (title, message) => {
    setModalMessage(`${title}: ${message}`);
    setModalVisible(true);
  };

  const handleCepChange = async (text) => {
    setCep(text);
    if (text.length === 8) {
      try {
        const response = await axios.get(`https://viacep.com.br/ws/${text}/json/`);
        if (response.data.erro) {
          showModal('CEP inválido', 'O CEP informado não foi encontrado');
          return;
        }
        setAddress({
          street: response.data.logradouro || '',
          neighborhood: response.data.bairro || '',
          city: response.data.localidade || '',
          state: response.data.uf || '',
          postalCode: text,
        });
      } catch (error) {
        showModal('Erro', 'Não foi possível buscar o endereço para o CEP informado');
      }
    }
  };

  const handleCalculatePrice = async () => {
    if (address.street.trim() && address.city.trim() && number.trim()) {
      const orderDto = {
        AddressOrigin: {
          StreetAddress: originAddress.street,
          NumberAddress: "123",
          AddressComplement: "",
          Neighborhood: originAddress.neighborhood,
          City: originAddress.city,
          PostalCode: originAddress.postalCode,
          Country: "Brasil",
          Latitude: originAddress.latitude,
          Longitude: originAddress.longitude,
        },
        AddressDestiny: {
          StreetAddress: address.street,
          NumberAddress: number,
          AddressComplement: complement,
          Neighborhood: address.neighborhood,
          City: address.city,
          PostalCode: cep,
          Country: "Brasil",
          Latitude: 0,
          Longitude: 0,
        },
        Items: [{ Weight: 2.5 }],
        DeliveryCost: null
      };

      try {
        const token = await AsyncStorage.getItem('token');
        console.log(token);
        const response = await axios.post(`${BASE_URL}/order/getDeliveryPrice`, orderDto, {
          headers: { Authorization: `Bearer ${token}` }
        });
        setDeliveryCost(response.data);
        setModalVisible(true);
      } catch (error) {
        console.error('API Call Error:', error);
        showModal('Erro', 'Não foi possível calcular o preço da entrega');
      }
    } else {
      showModal('Erro', 'Por favor, insira o endereço de entrega completo');
    }
  };

  const handleConfirmOrder = async () => {
    try {
      const token = await AsyncStorage.getItem('token');
      const customerId = await AsyncStorage.getItem('customerId');
      const orderDto = {
        CustomerId: customerId,
        AddressOrigin: originAddress,
        AddressDestiny: {
          ...address,
          NumberAddress: number,
          AddressComplement: complement,
        },
        Items: [{ Name: item, Weight: 2.5 }],
        DeliveryCost: deliveryCost,
      };

      console.log(orderDto);

      var response = await axios.post(`${BASE_URL}/order`, orderDto, {
        headers: { Authorization: `Bearer ${token}` }
      });



      Alert.alert('Pedido Confirmado', 'Seu pedido foi confirmado com sucesso!');
      setModalVisible(false);
      navigation.navigate('delivery-details/index', { item, address: JSON.stringify(response.data.destinationDelivery), id: response.data.id });
    } catch (error) {
      console.error('Erro ao confirmar pedido:', error);
      Alert.alert('Erro', 'Não foi possível confirmar o pedido. Tente novamente.');
    }
  };

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        <Text style={styles.title}>Digite o Endereço de Entrega para {item}</Text>
        <TextInput
          placeholder="CEP"
          value={cep}
          onChangeText={handleCepChange}
          style={styles.input}
          keyboardType="numeric"
          maxLength={8}
          placeholderTextColor="#999"
        />
        <TextInput
          placeholder="Rua"
          value={address.street}
          onChangeText={(text) => setAddress({ ...address, street: text })}
          style={styles.input}
          placeholderTextColor="#999"
        />
        <TextInput
          placeholder="Número"
          value={number}
          onChangeText={setNumber}
          style={styles.input}
          placeholderTextColor="#999"
        />
        <TextInput
          placeholder="Complemento"
          value={complement}
          onChangeText={setComplement}
          style={styles.input}
          placeholderTextColor="#999"
        />
        <TextInput
          placeholder="Bairro"
          value={address.neighborhood}
          onChangeText={(text) => setAddress({ ...address, neighborhood: text })}
          style={styles.input}
          placeholderTextColor="#999"
        />
        <TextInput
          placeholder="Cidade"
          value={address.city}
          onChangeText={(text) => setAddress({ ...address, city: text })}
          style={styles.input}
          placeholderTextColor="#999"
        />
        <TextInput
          placeholder="Estado"
          value={address.state}
          onChangeText={(text) => setAddress({ ...address, state: text })}
          style={styles.input}
          placeholderTextColor="#999"
        />
        <TouchableOpacity style={styles.button} onPress={handleCalculatePrice}>
          <Text style={styles.buttonText}>Calcular Preço</Text>
        </TouchableOpacity>
      </View>
      <Modal isVisible={isModalVisible} onBackdropPress={() => setModalVisible(false)}>
        <View style={styles.modalContent}>
          {deliveryCost !== null ? (
            <>
              <Text style={styles.modalText}>Item: {item}</Text>
              <Text style={styles.modalText}>Endereço da Entrega: {`${address.street}, ${number} - ${address.city}, ${address.state}`}</Text>
              <Text style={styles.modalText}>Valor da Entrega: {deliveryCost.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</Text>
              <TouchableOpacity style={styles.modalButton} onPress={handleConfirmOrder}>
                <Text style={styles.modalButtonText}>Efetuar Pedido</Text>
              </TouchableOpacity>
              <TouchableOpacity style={styles.modalButton} onPress={() => setModalVisible(false)}>
                <Text style={styles.modalButtonText}>Voltar</Text>
              </TouchableOpacity>
            </>
          ) : (
            <Text style={styles.modalText}>{modalMessage}</Text>
          )}
        </View>
      </Modal>
    </ImageBackground>
  );
}

const styles = StyleSheet.create({
  background: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  overlay: {
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    width: '100%',
    padding: 20,
  },
  title: {
    fontSize: 20,
    color: '#fff',
    marginBottom: 20,
    fontWeight: 'bold',
  },
  input: {
    width: '100%',
    padding: 10,
    borderWidth: 1,
    borderColor: '#ccc',
    marginBottom: 15,
    backgroundColor: '#fff',
    borderRadius: 8,
    color: '#000',
  },
  button: {
    backgroundColor: '#2196F3',
    padding: 15,
    borderRadius: 8,
    alignItems: 'center',
    marginTop: 10,
    width: '100%',
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold',
  },
  modalContent: {
    backgroundColor: 'white',
    padding: 20,
    borderRadius: 10,
    alignItems: 'center',
  },
  modalText: {
    fontSize: 16,
    marginBottom: 15,
    textAlign: 'center',
  },
  modalButton: {
    backgroundColor: '#2196F3',
    padding: 10,
    borderRadius: 8,
    marginTop: 10,
    width: '100%',
    alignItems: 'center',
  },
  modalButtonText: {
    color: '#fff',
    fontSize: 16,
  },
});
