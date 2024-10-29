import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, ImageBackground, ActivityIndicator, FlatList, Alert } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import logo from '../../assets/images/logo.jpg';
import { BASE_URL } from '../../config';

type Order = {
  deliveryCode: string;
  deliveryCost: number;
  deliveryStatus: string;
  orderId: string;
  destinationDelivery: {
    streetAddress: string;
    addressComplement: string;
    city: string;
    country: string;
    neighborhood: string;
    state: string;
  };
  dateHourOrder: string;
  orderDetailsItems: {
    name: string;
    description: string;
    weight: number;
  }[];
};

export default function OrderCustomer() {
  const navigation = useNavigation();
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const token = await AsyncStorage.getItem('token');
        const customerId = await AsyncStorage.getItem('customerId');
        const response = await axios.get(`${BASE_URL}/order/orders-customer`, {
          params: { id: customerId },
          headers: { Authorization: `Bearer ${token}` },
        });
        console.log(response.data.order);
        setOrders(response.data.order);
      } catch (error) {
        console.error('Erro ao obter pedidos:', error);
        Alert.alert('Erro', 'Não foi possível carregar os pedidos.');
      } finally {
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  const renderOrderItem = ({ item }: { item: Order }) => (
    <View style={styles.orderItem}>
      <Text style={styles.label}>Código de Entrega: {item.deliveryCode}</Text>
      <Text style={styles.label}>Status: {item.deliveryStatus}</Text>
      <Text style={styles.label}>Custo de Entrega: R$ {item.deliveryCost.toFixed(2)}</Text>
      <Text style={styles.label}>Data do Pedido: {new Date(item.dateHourOrder).toLocaleString()}</Text>
      <Text style={styles.label}>Destino:</Text>
      <Text style={styles.detail}>Rua: {item.destinationDelivery.streetAddress}</Text>
      <Text style={styles.detail}>Complemento: {item.destinationDelivery.addressComplement}</Text>
      <Text style={styles.detail}>Bairro: {item.destinationDelivery.neighborhood}</Text>
      <Text style={styles.detail}>Cidade: {item.destinationDelivery.city}</Text>
      <Text style={styles.detail}>Estado: {item.destinationDelivery.state}</Text>
      
      <Text style={styles.label}>Itens do Pedido:</Text>
      {item.orderDetailsItems.map((orderItem, index) => (
        <View key={index} style={styles.itemContainer}>
          <Text style={styles.detail}>Nome: {orderItem.name}</Text>
          <Text style={styles.detail}>Descrição: {orderItem.description}</Text>
          <Text style={styles.detail}>Peso: {orderItem.weight} kg</Text>
        </View>
      ))}
    </View>
  );

  if (loading) {
    return (
      <View style={styles.centered}>
        <ActivityIndicator size="large" color="#2196F3" />
      </View>
    );
  }

  if (orders.length === 0) {
    return (
      <View style={styles.centered}>
        <Text style={styles.errorText}>Nenhum pedido encontrado.</Text>
      </View>
    );
  }

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        <Text style={styles.title}>Meus Pedidos</Text>
        <FlatList
          data={orders}
          renderItem={renderOrderItem}
          keyExtractor={(item, index) => `${item.deliveryCode}-${index}`}
          contentContainerStyle={styles.listContent}
        />
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
    width: '100%',
    padding: 20,
  },
  centered: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  title: {
    fontSize: 24,
    color: '#fff',
    marginBottom: 20,
    fontWeight: 'bold',
    textAlign: 'center',
  },
  errorText: {
    color: 'red',
    fontSize: 18,
  },
  listContent: {
    paddingBottom: 20,
  },
  orderItem: {
    backgroundColor: '#fff',
    padding: 15,
    borderRadius: 8,
    marginBottom: 15,
  },
  itemContainer: {
    marginBottom: 8,
  },
  label: {
    fontSize: 16,
    color: '#333',
    fontWeight: 'bold',
    marginBottom: 4,
  },
  detail: {
    fontSize: 14,
    color: '#666',
    marginBottom: 4,
  },
});
