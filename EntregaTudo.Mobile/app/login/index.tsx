import React, { useState } from 'react';
import { View, TextInput, TouchableOpacity, Text, Alert, ImageBackground, StyleSheet } from 'react-native';
import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { LoginScreenNavigationProp } from '../../components/types';
import logo from '../../assets/images/logo.jpg';

type Props = {
  navigation: LoginScreenNavigationProp;
};

const LoginScreen: React.FC<Props> = ({ navigation }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = async () => {
    try {
      const response = await axios.post('https://45jgr80j-7174.brs.devtunnels.ms/login/login', {
        email,
        password,
      });
      const { token, customerId, userName, userType } = response.data;
      
      await AsyncStorage.setItem('token', token);
      await AsyncStorage.setItem('customerId', customerId);
      await AsyncStorage.setItem('userName', userName);
      await AsyncStorage.setItem('userType', userType);
      
      Alert.alert('Login realizado com sucesso');

      if (userType === 'DeliveryPerson') {
        navigation.navigate('delivery-person-home-screen/index');
      } else {
        navigation.navigate('home-screen/index');
      }
    } catch (error: unknown) {
      console.log(error);
      if (axios.isAxiosError(error)) {
        Alert.alert('Erro ao fazer login', error.response?.data?.message || 'Erro desconhecido');
      } else if (error instanceof Error) {
        Alert.alert('Erro ao fazer login', error.message);
      } else {
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
          placeholderTextColor="#999"
        />
        <TextInput
          placeholder="Senha"
          value={password}
          style={styles.input}
          secureTextEntry
          onChangeText={setPassword}
          placeholderTextColor="#999"
        />
        <TouchableOpacity style={styles.button} onPress={handleLogin}>
          <Text style={styles.buttonText}>Login</Text>
        </TouchableOpacity>
        <View style={styles.signupContainer}>
          <TouchableOpacity style={styles.button} onPress={() => navigation.navigate('SignupScreenCustomer')}>
            <Text style={styles.buttonText}>Cadastrar Usuário</Text>
          </TouchableOpacity>
          <TouchableOpacity style={styles.button} onPress={() => navigation.navigate('SignupScreenDeliveryPerson')}>
            <Text style={styles.buttonText}>Cadastrar Entregador</Text>
          </TouchableOpacity>
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
    color: '#000',
  },
  overlay: {
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    width: '100%',
    padding: 20,
  },
  button: {
    backgroundColor: '#2196F3',
    padding: 10,
    borderRadius: 5,
    alignItems: 'center',
    marginBottom: 10,
    width: '100%',
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
  },
  signupContainer: {
    marginTop: 20,
    width: '100%',
  },
});

export default LoginScreen;
