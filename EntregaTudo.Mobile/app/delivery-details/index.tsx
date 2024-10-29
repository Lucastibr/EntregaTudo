import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, ImageBackground, ActivityIndicator, Alert } from 'react-native';
import { useRoute } from '@react-navigation/native';
import axios from 'axios';
import logo from '../../assets/images/logo.jpg';
import { BASE_URL } from '../../config';
import AsyncStorage from '@react-native-async-storage/async-storage';

type Address = {
  street: string;
  neighborhood: string;
  city: string;
  state: string;
};

type OrderDetails = {
  deliveryCode: string;
  deliveryCost: number;
  destinationDelivery: Address;
  items: { name: string; description: string; weight: number }[];
};

export default function DeliveryDetailsScreen() {
  const route = useRoute();
  const { id } = route.params as { id: string }; // ID do pedido passado na navegação
  console.log(route.params);
  const [orderDetails, setOrderDetails] = useState<OrderDetails | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchOrderDetails = async () => {
      try {
        const token = await AsyncStorage.getItem('token');
        const response = await axios.get(`${BASE_URL}/order/getOrder`, {
          params: { id: id },
          headers: { Authorization: `Bearer ${token}` },
        });
        setOrderDetails(response.data);
      } catch (error) {
        console.error('Erro ao obter detalhes do pedido:', error);
        Alert.alert('Erro', 'Não foi possível carregar os detalhes do pedido.');
      } finally {
        setLoading(false);
      }
    };

    fetchOrderDetails();
  }, [id]);

  if (loading) {
    return (
      <View style={styles.centered}>
        <ActivityIndicator size="large" color="#2196F3" />
      </View>
    );
  }

  if (!orderDetails) {
    return (
      <View style={styles.centered}>
        <Text style={styles.errorText}>Erro ao carregar detalhes da entrega.</Text>
      </View>
    );
  }

  const { deliveryCode, deliveryCost, destinationDelivery, items } = orderDetails;

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        <Text style={styles.title}>Detalhes da Entrega</Text>
        <Text style={styles.label}>Código de Entrega: {deliveryCode}</Text>
        <Text style={styles.label}>Custo da Entrega: R$ {deliveryCost.toFixed(2)}</Text>
        <Text style={styles.label}>Endereço:</Text>
        <Text style={styles.detail}>Rua: {destinationDelivery.street}</Text>
        <Text style={styles.detail}>Bairro: {destinationDelivery.neighborhood}</Text>
        <Text style={styles.detail}>Cidade: {destinationDelivery.city}</Text>
        <Text style={styles.detail}>Estado: {destinationDelivery.state}</Text>
        <Text style={styles.label}>Itens:</Text>
        {items.map((item, index) => (
          <View key={index}>
            <Text style={styles.detail}>Nome: {item.name}</Text>
            <Text style={styles.detail}>Descrição: {item.description}</Text>
            <Text style={styles.detail}>Peso: {item.weight} kg</Text>
          </View>
        ))}
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
  centered: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  title: {
    fontSize: 20,
    color: '#fff',
    marginBottom: 20,
  },
  label: {
    fontSize: 18,
    color: '#fff',
    marginBottom: 8,
  },
  detail: {
    fontSize: 16,
    color: '#ccc',
    marginBottom: 8,
  },
  errorText: {
    color: 'red',
    fontSize: 18,
  },
});
