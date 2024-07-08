import React, { useEffect, useState } from 'react';
import { StyleSheet, View, Text, Button, ImageBackground } from 'react-native';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { StackNavigationProp } from '@react-navigation/stack';
import { RootStackParamList } from './types';
import logo from './images/logo.jpg';

type HomeScreenNavigationProp = StackNavigationProp<RootStackParamList, 'Home'>;

type Props = {
  navigation: HomeScreenNavigationProp;
};

export default function HomeScreen({ navigation }: Props) {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);

  useEffect(() => {
    const checkAuthStatus = async () => {
      try {
        const token = await AsyncStorage.getItem('token');
        console.log('Token:', token); // Adicionado para verificar o token
        setIsAuthenticated(!!token); // Atualize o estado baseado na existência do token
      } catch (error) {
        console.error('Erro ao obter o token:', error);
      }
    };

    checkAuthStatus();
  }, []);

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        <Text style={styles.title}>Entrega Tudo</Text>
        <Text style={styles.subtitle}>Seu delivery de tudo, a qualquer hora!</Text>
        {!isAuthenticated && (
          <Button
            title="Fazer Login"
            onPress={() => navigation.navigate('Login')}
          />
        )}
        {isAuthenticated && (
          <Button
            title="Ver Informações do Usuário"
            onPress={() => navigation.navigate('Profile')}
          />
        )}
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
    fontSize: 32,
    fontWeight: 'bold',
    color: '#fff',
    marginBottom: 16,
  },
  subtitle: {
    fontSize: 18,
    color: '#ccc',
    textAlign: 'center',
    marginBottom: 32,
  },
});
