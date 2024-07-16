import React, { useEffect, useState } from 'react';
import { View, Text, Alert, ImageBackground, StyleSheet, TouchableOpacity } from 'react-native';
import { useNavigation, useRoute } from '@react-navigation/native';
import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import logo from '../../assets/images/logo.jpg';

export default function ConfirmOrderScreen() {
  const navigation = useNavigation();
  const route = useRoute();
  const { item, address, deliveryCost } = route.params as {
    item: string;
    address: {
      street: string;
      neighborhood: string;
      city: string;
      state: string;
      postalCode: string;
    };
    deliveryCost: number;
  };
  const [customerId, setCustomerId] = useState<string | null>(null);

  useEffect(() => {
    const getCustomerId = async () => {
      const id = await AsyncStorage.getItem('customerId');
      setCustomerId(id);
    };

    getCustomerId();
  }, []);

  const handleConfirm = async () => {
    if (!customerId) {
      Alert.alert('Erro', 'Não foi possível recuperar o ID do cliente.');
      return;
    }

    const orderDto = {
      CustomerId: customerId,
      AddressOrigin: { PostalCode: '75384618' }, // CEP de origem fixo ou pode ser dinâmico
      AddressDestiny: {
        PostalCode: address.postalCode,
        AddressComplement: '',
        City: address.city,
        Country: 'BR',
        Latitude: 0,
        Longitude: 0,
        Neighborhood: address.neighborhood,
        NumberAddress: '',
        StreetAddress: address.street
      },
      Items: [{ Name: item, Description: '', Weight: 2.5 }], // Pode ser dinâmico com base nos itens selecionados
      DeliveryCost: deliveryCost
    };

    try {
      const token = await AsyncStorage.getItem('token');
      console.log(token);
      const response = await axios.post('https://rhq8kgxq-7174.brs.devtunnels.ms/order', orderDto, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      Alert.alert('Pedido Confirmado', 'Seu pedido foi confirmado com sucesso!');
    } catch (error) {
      console.log(error);
      Alert.alert('Erro', 'Não foi possível confirmar o pedido. Tente novamente.');
    }
  };

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        <Text style={styles.title}>Confirme seu Pedido</Text>
        <Text style={styles.label}>Item: {item}</Text>
        <Text style={styles.label}>Endereço: {`${address.street}, ${address.neighborhood}, ${address.city} - ${address.state}`}</Text>
        <Text style={styles.label}>Custo de Entrega: R$ {deliveryCost}</Text>
        <TouchableOpacity style={styles.button} onPress={handleConfirm}>
          <Text style={styles.buttonText}>Confirmar Pedido</Text>
        </TouchableOpacity>
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
  label: {
    fontSize: 16,
    color: '#fff',
    marginBottom: 10,
  },
  button: {
    backgroundColor: '#2196F3',
    paddingVertical: 10,
    paddingHorizontal: 20,
    borderRadius: 5,
    marginTop: 20,
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
  },
});
