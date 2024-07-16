import React from 'react';
import { View, Text, StyleSheet, ImageBackground } from 'react-native';
import { useRoute } from '@react-navigation/native';
import logo from '../../assets/images/logo.jpg'; // Certifique-se de que o caminho para a logo esteja correto

export default function DeliveryDetailsScreen() {
  const route = useRoute();
  const { item, address } = route.params as { item: string; address: string };
  const addressObj = JSON.parse(address);

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        <Text style={styles.title}>Detalhes da Entrega</Text>
        <Text style={styles.label}>Item: {item}</Text>
        <Text style={styles.label}>Endereço:</Text>
        <Text style={styles.detail}>Rua: {addressObj.street}</Text>
        <Text style={styles.detail}>Bairro: {addressObj.neighborhood}</Text>
        <Text style={styles.detail}>Cidade: {addressObj.city}</Text>
        <Text style={styles.detail}>Estado: {addressObj.state}</Text>
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
    fontSize: 18,
    color: '#fff',
    marginBottom: 8,
  },
  detail: {
    fontSize: 16,
    color: '#ccc',
    marginBottom: 8,
  },
});
