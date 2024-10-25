import React, { useEffect, useState } from 'react';
import { View, Text, FlatList, TouchableOpacity, StyleSheet, ImageBackground, Modal} from 'react-native';
import axios from 'axios';
import { useNavigation } from '@react-navigation/native';
import logo from '../../assets/images/logo.jpg';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { BASE_URL} from '../../config';

// Define the type for the order
interface Order {
  id: string;
  deliveryCode : string;
  address: {
    streetAddress: string;
    city: string;
    neighborhood: string;
    addressComplement: string;
    postalCode: string;
    latitude: number;
    longitude: number;
  };
  orderPrice: number;
}

export default function AvailableDeliveriesScreen() {
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedOrder, setSelectedOrder] = useState<Order | null>(null);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const navigation = useNavigation();

  useEffect(() => {
    const fetchAvailableOrders = async () => {
      try {
        const token = await AsyncStorage.getItem('token');
        const response = await axios.get(`${BASE_URL}/deliveryperson/available-orders`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setOrders(response.data.data);
        setLoading(false);
      } catch (error) {
        console.error('Error fetching available orders:', error);
        setLoading(false);
      }
    };

    fetchAvailableOrders();
  }, []);

  const handleSelectOrder = (order: Order) => {
    setSelectedOrder(order);
    setIsModalVisible(true);
  };

  const handleConfirmDelivery = () => {
    setIsModalVisible(false);
    if (selectedOrder) {
      navigation.navigate('delivery-order-details/index', { order: selectedOrder });
    }
  };

  const renderOrder = ({ item }: { item: Order }) => (
    <View style={styles.orderContainer}>
      <Text style={styles.orderText}>Endereço: {item.address.streetAddress}, {item.address.city}</Text>
      <Text style={styles.orderText}>Bairro: {item.address.neighborhood}</Text>
      <Text style={styles.orderText}>Complemento: {item.address.addressComplement}</Text>
      <Text style={styles.orderText}>CEP: {item.address.postalCode}</Text>
      <Text style={styles.orderText}>
        Valor: {parseFloat(item.orderPrice.toFixed(2)).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
      </Text>
      <TouchableOpacity
        style={styles.button}
        onPress={() => handleSelectOrder(item)}
      >
        <Text style={styles.buttonText}>Selecionar Entrega</Text>
      </TouchableOpacity>
    </View>
  );

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        {loading ? (
          <Text style={styles.loadingText}>Carregando entregas disponíveis...</Text>
        ) : (
          <FlatList
            data={orders}
            renderItem={renderOrder}
            keyExtractor={(item) => item.deliveryCode}
            contentContainerStyle={styles.listContainer}
          />
        )}

        <Modal
          transparent={true}
          visible={isModalVisible}
          animationType="slide"
          onRequestClose={() => setIsModalVisible(false)}
        >
          <View style={styles.modalOverlay}>
            <View style={styles.modalContainer}>
              <Text style={styles.modalTitle}>Confirmar Entrega</Text>
              <Text style={styles.modalText}>Deseja confirmar esta entrega?</Text>
              <View style={styles.modalButtons}>
                <TouchableOpacity
                  style={styles.button}
                  onPress={() => setIsModalVisible(false)}
                >
                  <Text style={styles.buttonText}>Cancelar</Text>
                </TouchableOpacity>
                <TouchableOpacity
                  style={styles.button}
                  onPress={handleConfirmDelivery}
                >
                  <Text style={styles.buttonText}>Confirmar</Text>
                </TouchableOpacity>
              </View>
            </View>
          </View>
        </Modal>
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
  listContainer: {
    paddingBottom: 20,
  },
  orderContainer: {
    backgroundColor: '#fff',
    padding: 15,
    borderRadius: 5,
    marginBottom: 15,
    width: '100%',
  },
  orderText: {
    fontSize: 16,
    color: '#333',
    marginBottom: 5,
  },
  button: {
    backgroundColor: '#2196F3',
    padding: 10,
    borderRadius: 5,
    alignItems: 'center',
    marginTop: 10,
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
  },
  loadingText: {
    fontSize: 18,
    color: '#fff',
  },
  modalOverlay: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
  },
  modalContainer: {
    backgroundColor: '#fff',
    padding: 20,
    borderRadius: 10,
    width: '80%',
    alignItems: 'center',
  },
  modalTitle: {
    fontSize: 20,
    fontWeight: 'bold',
    marginBottom: 10,
  },
  modalText: {
    fontSize: 16,
    marginBottom: 20,
  },
  modalButtons: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    width: '100%',
  },
});