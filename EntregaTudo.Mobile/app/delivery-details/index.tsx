import React from 'react';
import { View, Text, Button, StyleSheet, Alert } from 'react-native';
import { useRoute } from '@react-navigation/native';

export default function DeliveryDetailsScreen() {
  const route = useRoute();
  const { item, address } = route.params as { item: string; address: string };

  const calculateDelivery = () => {
    const value = (Math.random() * 100).toFixed(2); // Valor fictício
    const time = Math.floor(Math.random() * 60); // Tempo estimado fictício
    Alert.alert('Detalhes da Entrega', `Valor: R$ ${value}\nTempo estimado: ${time} minutos`);
  };

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Detalhes da Entrega</Text>
      <Text>Item: {item}</Text>
      <Text>Endereço: {address}</Text>
      <Button title="Calcular Entrega" onPress={calculateDelivery} />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
  },
  title: {
    fontSize: 20,
    marginBottom: 20,
  },
});
