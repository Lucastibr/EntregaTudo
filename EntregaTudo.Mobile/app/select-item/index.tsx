import React, { useState } from 'react';
import { View, Text, Button, Alert, StyleSheet } from 'react-native';
import { useNavigation } from '@react-navigation/native';

export default function SelectItemScreen() {
  const navigation = useNavigation();
  const [item, setItem] = useState<string>('');

  const handleNext = () => {
    if (item.trim()) {
      navigation.navigate('enter-address/index', { item });
    } else {
      Alert.alert('Por favor, insira o item para entrega');
    }
  };

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Selecione o Item para Entrega</Text>
      <Button title="Documento" onPress={() => setItem('Documento')} />
      <Button title="Próximo" onPress={handleNext} />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  title: {
    fontSize: 20,
    marginBottom: 20,
  },
});
