import React, { useEffect, useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  Linking,
  ImageBackground,
  Alert,
} from 'react-native';
import { KeyboardAwareScrollView } from 'react-native-keyboard-aware-scroll-view';
import logo from '../../assets/images/logo.jpg';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { useNavigation } from '@react-navigation/native';
import { BASE_URL } from '../../config';
import axios from 'axios';

export default function DeliveryDetailScreen({ route }: any) {
  const { order } = route.params;
  const [deliveryCode, setDeliveryCode] = useState('');
  const navigation = useNavigation();

  // Função para formatar o número de telefone
  const formatPhoneNumber = (phoneNumber) => {
    const cleaned = phoneNumber.replace(/\D/g, ''); // Remove caracteres não numéricos
    return `+55${cleaned}`; // Ajuste o formato conforme necessário
  };

  // Função para enviar mensagem via API C#
  const sendMessageToApi = async () => {
    const formattedPhoneNumber = formatPhoneNumber(order.phoneNumber);
    const message = `Olá, ${order.customerName}! Seu pedido está a caminho! A entrega será feita por ${order.deliveryPersonName}, Placa ${order.licensePlate}! Fique ligado!`;
    console.log(formattedPhoneNumber);
    console.log(message);
    const response = await fetch(
      `${BASE_URL}/deliveryPerson/send?phoneNumber=${formattedPhoneNumber}&message=${message}`,
      {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
      }
    );

    if (response.ok) {
     
    } else {
      const text = await response.text();
      console.log(text)
      const errorData = JSON.parse(text);
      Alert.alert('Erro', errorData.message || 'Erro ao finalizar o pedido.');
    }
  };

  // Enviar mensagem ao carregar a tela
  useEffect(() => {
    sendMessageToApi();
  }, []);

  const handleOpenWaze = async () => {
    try {
      const response = await axios.get(
        `https://cep.awesomeapi.com.br/json/${order.address.postalCode}`
      );
      const { lat, lng } = response.data;
      const wazeUrl = `https://waze.com/ul?ll=${lat},${lng}&navigate=yes`;
      Linking.openURL(wazeUrl);
    } catch (error) {
      Alert.alert('Erro', 'Não foi possível abrir o Waze.');
    }
  };

  const handleConfirmDeliveryCode = async () => {
    if (!deliveryCode) {
      Alert.alert('Erro', 'Por favor, informe o código de entrega.');
      return;
    }

    try {
      const token = await AsyncStorage.getItem('token');
      const id = await AsyncStorage.getItem('customerId');
      const response = await fetch(
        `${BASE_URL}/order/finalizeOrder?orderId=${order.id}&id=${id}&deliveryCode=${deliveryCode}`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.ok) {
        Alert.alert('Sucesso', 'Pedido finalizado com sucesso.', [
          {
            text: 'OK',
            onPress: () => navigation.navigate('delivery-person-home-screen/index'),
          },
        ]);
      } else {
        const text = await response.text();
        const errorData = JSON.parse(text);
        Alert.alert('Erro', errorData.message || 'Erro ao finalizar o pedido.');
      }
    } catch (error) {
      Alert.alert('Erro', 'Erro de rede ou problema com o servidor.');
    }
  };

  return (
    <ImageBackground source={logo} style={styles.background}>
      <KeyboardAwareScrollView
        contentContainerStyle={styles.scrollContainer}
        enableOnAndroid={true}
        extraScrollHeight={100}
      >
        <View style={styles.card}>
          <Text style={styles.title}>Detalhes do Pedido</Text>
          <View style={styles.infoContainer}>
            <Text style={styles.label}>Endereço:</Text>
            <Text style={styles.value}>
              {order.address.streetAddress}, {order.address.city}
            </Text>
          </View>
          <View style={styles.infoContainer}>
            <Text style={styles.label}>Bairro:</Text>
            <Text style={styles.value}>{order.address.neighborhood}</Text>
          </View>
          <View style={styles.infoContainer}>
            <Text style={styles.label}>Complemento:</Text>
            <Text style={styles.value}>{order.address.addressComplement}</Text>
          </View>
          <View style={styles.infoContainer}>
            <Text style={styles.label}>CEP:</Text>
            <Text style={styles.value}>{order.address.postalCode}</Text>
          </View>
          <TouchableOpacity style={styles.wazeButton} onPress={handleOpenWaze}>
            <Text style={styles.buttonText}>Abrir no Waze</Text>
          </TouchableOpacity>
        </View>

        <View style={styles.card}>
          <Text style={styles.title}>Finalizar Pedido</Text>
          <TextInput
            style={styles.input}
            placeholder="Digite o código de entrega"
            value={deliveryCode}
            onChangeText={setDeliveryCode}
          />
          <TouchableOpacity style={styles.confirmButton} onPress={handleConfirmDeliveryCode}>
            <Text style={styles.buttonText}>Confirmar Código</Text>
          </TouchableOpacity>
        </View>
      </KeyboardAwareScrollView>
    </ImageBackground>
  );
}

const styles = StyleSheet.create({
  background: {
    flex: 1,
  },
  scrollContainer: {
    padding: 20,
  },
  card: {
    backgroundColor: '#fff',
    borderRadius: 10,
    padding: 20,
    marginBottom: 20,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.2,
    shadowRadius: 5,
    elevation: 5,
  },
  title: {
    fontSize: 20,
    fontWeight: 'bold',
    marginBottom: 15,
    color: '#333',
  },
  infoContainer: {
    marginBottom: 10,
  },
  label: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#555',
  },
  value: {
    fontSize: 16,
    color: '#333',
    marginTop: 3,
  },
  wazeButton: {
    backgroundColor: '#34A853',
    padding: 15,
    borderRadius: 8,
    alignItems: 'center',
    marginTop: 20,
  },
  confirmButton: {
    backgroundColor: '#2196F3',
    padding: 15,
    borderRadius: 8,
    alignItems: 'center',
    marginTop: 20,
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold',
  },
  input: {
    borderWidth: 1,
    borderColor: '#ccc',
    padding: 15,
    borderRadius: 8,
    fontSize: 16,
    marginTop: 10,
  },
});
