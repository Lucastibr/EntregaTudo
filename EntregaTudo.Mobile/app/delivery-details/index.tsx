import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, ImageBackground, ActivityIndicator, Alert, TouchableOpacity } from 'react-native';
import { useRoute, useNavigation } from '@react-navigation/native';
import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import logo from '../../assets/images/logo.jpg';
import { BASE_URL } from '../../config';

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
  const navigation = useNavigation();
  const { id } = route.params as { id: string };
  const [orderDetails, setOrderDetails] = useState<OrderDetails | null>(null);
  const [loading, setLoading] = useState(true);
  // Configuração de navegação para desabilitar o botão de voltar
  useEffect(() => {
    navigation.setOptions({
      headerLeft: () => null,  // Remove o botão de voltar
      gestureEnabled: false,   // Desativa o gesto de voltar
    });
  }, [navigation]);

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
        <ActivityIndicator size="large" color="#007BFF" />
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
        <Text style={styles.title}>📦 Detalhes do Envio da sua Encomenda</Text>
        <View style={styles.infoBox}>
          <Text style={styles.label}>Código de Entrega:</Text>
          <Text style={styles.infoText}>{deliveryCode}</Text>
        </View>
        <View style={styles.infoBox}>
          <Text style={styles.label}>Custo da Entrega:</Text>
          <Text style={styles.infoText}>R$ {deliveryCost.toFixed(2)}</Text>
        </View>
        
        <Text style={styles.sectionTitle}>Endereço de Destino</Text>
        <View style={styles.addressBox}>
          <Text style={styles.detail}>📍 Rua: {destinationDelivery.street}</Text>
          <Text style={styles.detail}>🏘 Bairro: {destinationDelivery.neighborhood}</Text>
          <Text style={styles.detail}>🏙 Cidade: {destinationDelivery.city}</Text>
          <Text style={styles.detail}>🌍 Estado: {destinationDelivery.state}</Text>
        </View>

        <Text style={styles.sectionTitle}>Itens:</Text>
        {items.map((item, index) => (
          <View key={index} style={styles.itemBox}>
            <Text style={styles.detail}>📦 Nome: {item.name}</Text>
            <Text style={styles.detail}>📝 Descrição: {item.description}</Text>
            <Text style={styles.detail}>⚖️ Peso: {item.weight} kg</Text>
          </View>
        ))}

        <Text style={styles.paymentMessage}>
          💰 O pedido deve ser pago na hora que o entregador buscar.
        </Text>

        <TouchableOpacity
          style={styles.button}
          onPress={() => navigation.navigate('orders-customers/index')}
        >
          <Text style={styles.buttonText}>Ver Meus Pedidos</Text>
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
    backgroundColor: 'rgba(0, 0, 0, 0.6)',
    flex: 1,
    width: '90%',
    borderRadius: 15,
    padding: 20,
    marginVertical: 30,
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
  infoBox: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    backgroundColor: '#FFF',
    borderRadius: 8,
    padding: 10,
    marginBottom: 8,
  },
  label: {
    fontSize: 16,
    color: '#555',
    fontWeight: 'bold',
  },
  infoText: {
    fontSize: 16,
    color: '#007BFF',
    fontWeight: 'bold',
  },
  sectionTitle: {
    fontSize: 18,
    color: '#FFF',
    fontWeight: 'bold',
    marginTop: 15,
    marginBottom: 8,
    textAlign: 'center',
  },
  addressBox: {
    backgroundColor: '#FFF',
    padding: 10,
    borderRadius: 8,
    marginBottom: 12,
  },
  itemBox: {
    backgroundColor: '#FFF',
    padding: 10,
    borderRadius: 8,
    marginBottom: 10,
  },
  detail: {
    fontSize: 14,
    color: '#333',
    marginBottom: 4,
  },
  paymentMessage: {
    fontSize: 16,
    color: '#FFD700',
    marginTop: 20,
    textAlign: 'center',
    fontWeight: 'bold',
  },
  button: {
    backgroundColor: '#007BFF',
    paddingVertical: 12,
    borderRadius: 8,
    marginTop: 25,
  },
  buttonText: {
    color: '#FFF',
    fontSize: 16,
    fontWeight: 'bold',
    textAlign: 'center',
  },
  errorText: {
    color: 'red',
    fontSize: 18,
  },
});
