import React from 'react';
import { View, Text, Button, StyleSheet,ImageBackground } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import logo from '../../assets/images/logo.jpg';

export default function HomeScreen() {
  const navigation = useNavigation();

  return (
    <ImageBackground source={logo} style={styles.background}>
    <View style={styles.container}>
      <Text style={styles.title}>Bem-vindo ao App de Delivery</Text>
      <Button title="Selecionar Item"  onPress={() => navigation.navigate('select-item/index')} />
    </View>
    </ImageBackground>
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
  background: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  input: {
    width: '100%',
    height: 40,
    borderColor: '#ccc',
    borderWidth: 1,
    marginBottom: 16,
    paddingHorizontal: 8,
    backgroundColor: '#fff',
    borderRadius: 5,
  },
});
