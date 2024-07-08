import React, { useState } from 'react';
import { View, TextInput, Button, Alert, ImageBackground, StyleSheet } from 'react-native';
import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { LoginScreenNavigationProp } from './types';
import logo from './images/logo.jpg';

type Props = {
  navigation: LoginScreenNavigationProp;
};

const LoginScreen: React.FC<Props> = ({ navigation }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = async () => {
    try {
      const response = await axios.post('http://10.0.2.2:5240/login/login', {
        email,
        password,
      });
      await AsyncStorage.setItem('token', response.data.token);
      Alert.alert('Login realizado com sucesso');
      console.log(response.data.token);
      navigation.navigate('Profile');
    } catch (error: unknown) {
      console.log(error);
      if (axios.isAxiosError(error)) {
        // Axios-specific error
        Alert.alert('Erro ao fazer login', error.response?.data?.message || 'Erro desconhecido');
      } else if (error instanceof Error) {
        // Native Error instance
        Alert.alert('Erro ao fazer login', error.message);
      } else {
        // Other unexpected errors
        Alert.alert('Erro ao fazer login', 'Erro desconhecido');
      }
    }
  };

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        <TextInput
          placeholder="Email"
          style={styles.input}
          value={email}
          onChangeText={setEmail}
        />
        <TextInput
          placeholder="Senha"
          value={password}
          style={styles.input}
          secureTextEntry
          onChangeText={setPassword}
        />
        <Button title="Login" onPress={handleLogin} />
        <View style={styles.signupContainer}>
          <Button
            title="Cadastrar Usuário"
            onPress={() => navigation.navigate('SignupScreenCustomer')}
          />
          <Button
            title="Cadastrar Entregador"
            onPress={() => navigation.navigate('SignupScreenDeliveryPerson')}
          />
        </View>
      </View>
    </ImageBackground>
  );
};

const styles = StyleSheet.create({
  background: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  input: {
    width: '100%',
    backgroundColor: '#fff',
    padding: 10,
    marginBottom: 16,
    borderRadius: 5,
  },
  overlay: {
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    width: '100%',
    padding: 20,
  },
  signupContainer: {
    marginTop: 20,
    width: '100%',
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
});

export default LoginScreen;
