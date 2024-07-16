import React, { useState } from 'react';
import { View, Text, TextInput, Button, StyleSheet, Alert, ImageBackground } from 'react-native';
import { useNavigation, useRoute } from '@react-navigation/native';
import axios from 'axios';
import logo from '../../assets/images/logo.jpg';

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
  });
  const [number, setNumber] = useState('');
  const [complement, setComplement] = useState('');

  const handleCepChange = async (text: string) => {
    setCep(text);
    if (text.length === 8) {
      try {
        const response = await axios.get(`https://viacep.com.br/ws/${text}/json/`);
        if (response.data.erro) {
          Alert.alert('CEP inválido', 'O CEP informado não foi encontrado');
          return;
        }
        setAddress({
          street: response.data.logradouro || '',
          neighborhood: response.data.bairro || '',
          city: response.data.localidade || '',
          state: response.data.uf || '',
        });
      } catch (error) {
        Alert.alert('Erro ao buscar endereço', 'Não foi possível buscar o endereço para o CEP informado');
      }
    }
  };

  const handleNext = () => {
    if (address.street.trim() && address.city.trim() && number.trim()) {
      navigation.navigate('delivery-details/index', { item, address: JSON.stringify(address) });
    } else {
      Alert.alert('Por favor, insira o endereço de entrega completo');
    }
  };

  const handleCalculatePrice = async () => {
    if (address.street.trim() && address.city.trim() && number.trim()) {
      const orderDto = {
        AddressOrigin: {
          StreetAddress: "Rua Fictícia", // Endereço de origem fictício ou dinâmico
          NumberAddress: "123",
          AddressComplement: "",
          Neighborhood: "Bairro Fictício",
          City: "Cidade Fictícia",
          PostalCode: "75384618",
          Country: "Brasil",
          Latitude: 0,
          Longitude: 0
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
          Longitude: 0
        },
        Items: [
          {
            Weight: 2.5 // Peso fictício do item ou pode ser dinâmico
          }
        ],
        DeliveryCost: null
      };

      try {
        console.log('Sending request to API with data:', orderDto); // Logging data for debugging
        const response = await axios.post('https://rhq8kgxq-7174.brs.devtunnels.ms/order/getDeliveryPrice', orderDto);
        console.log('API Response:', response.data); // Logging response for debugging
        Alert.alert('Preço do Delivery', `O valor do delivery é: R$${response.data}`);
      } catch (error) {
        console.error('API Call Error:', error); // Logging error for debugging
        Alert.alert('Erro ao calcular preço', 'Não foi possível calcular o preço da entrega');
      }
    } else {
      Alert.alert('Por favor, insira o endereço de entrega completo');
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
        <Button title="Calcular Preço" onPress={handleCalculatePrice} />
        <Button title="Próximo" onPress={handleNext} />
      </View>
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
  },
  input: {
    width: '100%',
    padding: 10,
    borderWidth: 1,
    borderColor: '#ccc',
    marginBottom: 20,
    backgroundColor: '#fff',
    color: '#000',
    zIndex: 1,
  },
});