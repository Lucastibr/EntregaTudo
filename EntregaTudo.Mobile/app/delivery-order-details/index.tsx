import React, { useState } from 'react';
import { View, Text, TextInput, TouchableOpacity, StyleSheet, Linking,ImageBackground, Alert } from 'react-native';
import logo from '../../assets/images/logo.jpg';
import AsyncStorage from '@react-native-async-storage/async-storage';

export default function DeliveryDetailScreen({ route }: any) {
  const { order } = route.params;
  console.log(order);
  const [deliveryCode, setDeliveryCode] = useState('');

  const handleOpenWaze = () => {
    console.log(order.address.latitude);
    console.log(order.address.longitude);
    const wazeUrl = `https://waze.com/ul?ll=${order.address.latitude},${order.address.longitude}&navigate=yes`;
    Linking.openURL(wazeUrl);
  };

  const handleConfirmDeliveryCode = async () => {
    console.log(`Código de entrega inserido: ${deliveryCode}`);
    console.log(order);
  
    // Validações antes de chamar a API
    if (!order || !order.id) {
      Alert.alert("Erro", "ID do pedido não encontrado.");
      return;
    }
  
    if (deliveryCode === null || deliveryCode === "") {
      Alert.alert("Erro", "Código Não Informado!");
      return;
    }

    try {
      var token = await AsyncStorage.getItem('token');
      console.log(token);
      const response = await fetch(`https://45jgr80j-7174.brs.devtunnels.ms/order/finalizeOrder?id=${order.id}&deliveryCode=${deliveryCode}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
      });
  
      if (response.ok) {
        Alert.alert("Sucesso", "Pedido finalizado com sucesso.");
      } else {
        // Verifica se há um corpo na resposta
        const text = await response.text();
        console.log(text);
        let errorData;
        
        try {
          errorData = JSON.parse(text);
        } catch (e) {
          console.error("Erro ao analisar JSON:", e);
          Alert.alert("Erro", "Erro ao finalizar o pedido. Resposta inesperada do servidor.");
          return;
        }
  
        Alert.alert("Erro", errorData.message || "Erro ao finalizar o pedido.");
      }
    } catch (error) {
      console.error("Erro ao chamar a API:", error);
      Alert.alert("Erro", "Erro de rede ou problema com o servidor.");
    }
  };
  
  return (
    <ImageBackground source={logo} style={styles.background}>
    <View style={styles.container}>
      <Text style={styles.label}>Endereço:</Text>
      <Text style={styles.value}>{order.address.streetAddress}, {order.address.city}</Text>
      <Text style={styles.label}>Bairro:</Text>
      <Text style={styles.value}>{order.address.neighborhood}</Text>
      <Text style={styles.label}>Complemento:</Text>
      <Text style={styles.value}>{order.address.addressComplement}</Text>
      <Text style={styles.label}>CEP:</Text>
      <Text style={styles.value}>{order.address.postalCode}</Text>

      <TouchableOpacity style={styles.button} onPress={handleOpenWaze}>
        <Text style={styles.buttonText}>Abrir no Waze</Text>
      </TouchableOpacity>

      <TextInput
        style={styles.input}
        placeholder="Digite o código de entrega"
        value={deliveryCode}
        onChangeText={setDeliveryCode}
      />

      <TouchableOpacity style={styles.button} onPress={handleConfirmDeliveryCode}>
        <Text style={styles.buttonText}>Confirmar Código</Text>
      </TouchableOpacity>
    </View>
    </ImageBackground>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
  },
  background: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  label: {
    fontSize: 18,
    fontWeight: 'bold',
    marginBottom: 5,
  },
  value: {
    fontSize: 16,
    marginBottom: 15,
  },
  button: {
    backgroundColor: '#2196F3',
    padding: 10,
    borderRadius: 5,
    alignItems: 'center',
    marginTop: 20,
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
  },
  input: {
    borderWidth: 1,
    borderColor: '#ccc',
    padding: 10,
    borderRadius: 5,
    marginTop: 20,
    fontSize: 16,
  },
});