import React from 'react';
import { View, Text, Button, StyleSheet, Alert, ImageBackground } from 'react-native';
import { useNavigation, useRoute } from '@react-navigation/native';
import axios from 'axios';
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

  const handleConfirm = async () => {
    const orderDto = {
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
      const response = await axios.post('http://192.168.1.100:5240/order', orderDto);
      Alert.alert('Pedido Confirmado', 'Seu pedido foi confirmado com sucesso!');
      navigation.navigate('home-screen/index'); // Redireciona para a tela inicial
    } catch (error) {
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
        <Button title="Confirmar Pedido" onPress={handleConfirm} />
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
});
