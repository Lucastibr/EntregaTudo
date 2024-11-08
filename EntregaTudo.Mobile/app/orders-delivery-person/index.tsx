import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, ImageBackground, ActivityIndicator, FlatList, Alert } from 'react-native';
import { MaterialCommunityIcons } from '@expo/vector-icons'; // Importa ícones do Expo
import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import logo from '../../assets/images/logo.jpg';
import { BASE_URL } from '../../config';

type Order = {
  deliveryCost: number;
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

export default function DeliveredOrdersScreen() {
  const [orders, setOrders] = useState<Order[]>([]);
  const [totalEarnings, setTotalEarnings] = useState(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const token = await AsyncStorage.getItem('token');
        const deliveryPersonId = await AsyncStorage.getItem('customerId');
        const response = await axios.get(`${BASE_URL}/order/orders-delivery-person`, {
          params: { id: deliveryPersonId },
          headers: { Authorization: `Bearer ${token}` },
        });
        
        const fetchedOrders = response.data.order;
        setOrders(fetchedOrders);

        // Calcula o total de ganhos somando todos os deliveryCost dos pedidos
        const total = fetchedOrders.reduce((sum: number, order: Order) => sum + order.deliveryCost, 0);
        setTotalEarnings(total);
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
      <Text style={styles.label}>Custo da Entrega: R$ {item.deliveryCost.toFixed(2)}</Text>
      <Text style={styles.label}>Data da Entrega: {new Date(item.dateHourOrder).toLocaleString()}</Text>
      <Text style={styles.label}>Destino:</Text>
      <Text style={styles.detail}>Rua: {item.destinationDelivery.streetAddress}</Text>
      <Text style={styles.detail}>Complemento: {item.destinationDelivery.addressComplement}</Text>
      <Text style={styles.detail}>Bairro: {item.destinationDelivery.neighborhood}</Text>
      <Text style={styles.detail}>Cidade: {item.destinationDelivery.city}</Text>
      <Text style={styles.detail}>Estado: {item.destinationDelivery.state}</Text>

      <Text style={styles.label}>Itens:</Text>
      {item.orderDetailsItems.map((orderItem, index) => (
        <View key={index} style={styles.itemContainer}>
          <Text style={styles.detail}>📦 Nome: {orderItem.name}</Text>
          <Text style={styles.detail}>📝 Descrição: {orderItem.description}</Text>
          <Text style={styles.detail}>⚖️ Peso: {orderItem.weight} kg</Text>
        </View>
      ))}
    </View>
  );

  if (loading) {
    return (
      <View style={styles.centered}>
        <ActivityIndicator size="large" color="#007BFF" />
      </View>
    );
  }

  if (orders.length === 0) {
    return (
      <ImageBackground source={logo} style={styles.background}>
        <View style={styles.overlay}>
          {/* Ícone de moto */}
          <MaterialCommunityIcons name="motorbike" size={100} color="#FFD700" />
          <Text style={styles.noOrdersText}>Nenhuma entrega encontrada</Text>
          <Text style={styles.subText}>Você ainda não fez nenhuma entrega. Aguarde novas encomendas para começar a entregar!</Text>
        </View>
      </ImageBackground>
    );
  }

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        <Text style={styles.title}>📈 Pedidos Entregues</Text>
        <Text style={styles.totalEarnings}>Total Ganho: R$ {totalEarnings.toFixed(2)}</Text>
        
        <FlatList
          data={orders}
          renderItem={renderOrderItem}
          keyExtractor={(item, index) => `${item.dateHourOrder}-${index}`}
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
    backgroundColor: 'rgba(0, 0, 0, 0.7)',
    flex: 1,
    width: '90%',
    borderRadius: 15,
    padding: 20,
    marginVertical: 30,
    alignItems: 'center',
  },
  centered: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  title: {
    fontSize: 22,
    color: '#FFF',
    marginBottom: 20,
    fontWeight: 'bold',
    textAlign: 'center',
  },
  totalEarnings: {
    fontSize: 20,
    color: '#FFD700',
    fontWeight: 'bold',
    textAlign: 'center',
    marginBottom: 15,
  },
  errorText: {
    color: 'red',
    fontSize: 18,
  },
  listContent: {
    paddingBottom: 20,
  },
  orderItem: {
    backgroundColor: '#FFF',
    padding: 15,
    borderRadius: 8,
    marginBottom: 15,
    width: '100%',
  },
  label: {
    fontSize: 16,
    color: '#333',
    fontWeight: 'bold',
    marginBottom: 4,
  },
  itemContainer: {
    marginBottom: 8,
  },
  detail: {
    fontSize: 14,
    color: '#666',
    marginBottom: 4,
  },
  noOrdersText: {
    fontSize: 20,
    color: '#FFD700',
    fontWeight: 'bold',
    textAlign: 'center',
    marginTop: 20,
  },
  subText: {
    fontSize: 16,
    color: '#FFF',
    textAlign: 'center',
    marginTop: 10,
    paddingHorizontal: 20,
  },
});
