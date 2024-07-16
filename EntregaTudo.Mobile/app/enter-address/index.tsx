import React, { useState } from 'react';
import { View, Text, TextInput, Button, StyleSheet, Alert } from 'react-native';
import { useNavigation, useRoute } from '@react-navigation/native';

export default function EnterAddressScreen() {
  const navigation = useNavigation();
  const route = useRoute();
  const { item } = route.params as { item: string };

  const [address, setAddress] = useState('');

  const handleNext = () => {
    if (address.trim()) {
      navigation.navigate('DeliveryDetails', { item, address });
    } else {
      Alert.alert('Por favor, insira o endereço de entrega');
    }
  };

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Digite o Endereço de Entrega para {item}</Text>
      <TextInput
        placeholder="Endereço"
        value={address}
        onChangeText={setAddress}
        style={styles.input}
      />
      <Button title="Próximo" onPress={handleNext} />
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
  input: {
    width: '100%',
    padding: 10,
    borderWidth: 1,
    borderColor: '#ccc',
    marginBottom: 20,
  },
});
