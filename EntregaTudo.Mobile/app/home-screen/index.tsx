import React, { useEffect, useState, useCallback } from 'react';
import { View, Text, Button, StyleSheet, ImageBackground, TouchableOpacity } from 'react-native';
import { useNavigation, useFocusEffect } from '@react-navigation/native';
import AsyncStorage from '@react-native-async-storage/async-storage';
import logo from '../../assets/images/logo.jpg';

export default function HomeScreen() {
  const navigation = useNavigation();
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);

  const checkAuthStatus = async () => {
    try {
      const token = await AsyncStorage.getItem('token');
      const userType = await AsyncStorage.getItem('userType');
      setIsAuthenticated(!!token); 

      if (userType === 'DeliveryPerson') {
        navigation.navigate('delivery-person-home-screen/index');
      } 
      
    } catch (error) {
      console.error('Error retrieving token:', error);
    }
  };

  useFocusEffect(
    useCallback(() => {
      checkAuthStatus();
    }, [])
  );

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.container}>
        <Text style={styles.title}>Entrega Tudo</Text>
        <Text style={styles.subtitle}>Seu delivery de tudo, a qualquer hora!</Text>
        {isAuthenticated ? (
          <>
            <Text style={styles.welcomeText}>Bem-vindo!</Text>
            <TouchableOpacity
              style={styles.button}
              onPress={() => navigation.navigate('select-item/index')}
            >
              <Text style={styles.buttonText}>Selecionar Item</Text>
            </TouchableOpacity>
          </>
        ) : (
          <TouchableOpacity
            style={styles.button}
            onPress={() => navigation.navigate('login/index')}
          >
            <Text style={styles.buttonText}>Login</Text>
          </TouchableOpacity>
        )}
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
    color: '#fff', 
  },
  background: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    width: '100%',
  },
  button: {
    backgroundColor: '#2196F3',
    padding: 10,
    borderRadius: 5,
    alignItems: 'center',
    marginTop: 10,
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
  },
  subtitle: {
    fontSize: 18,
    color: '#ccc',
    textAlign: 'center',
    marginBottom: 32,
  },
  welcomeText: {
    fontSize: 18,
    color: '#fff',
    marginBottom: 20,
  },
});
