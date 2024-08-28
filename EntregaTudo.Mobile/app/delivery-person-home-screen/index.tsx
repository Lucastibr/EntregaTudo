import React, { useEffect, useState } from 'react';
import { View, Text, Button, StyleSheet, TouchableOpacity,ImageBackground,Alert  } from 'react-native';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { useNavigation } from '@react-navigation/native';
import logo from '../../assets/images/logo.jpg';

export default function DeliveryWelcomeScreen() {
  const [userName, setUserName] = useState<string | null>(null);
  const navigation = useNavigation();

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const name = await AsyncStorage.getItem('userName');
        const userType = await AsyncStorage.getItem('userType');

        if (userType === "DeliveryPerson") {
          setUserName(name);
          navigation.navigate('delivery-person-home-screen/index');
        } else {
          navigation.navigate('home-screen/index'); // Redirect if not a delivery person
        }
      } catch (error) {
        console.error('Error fetching user data:', error);
      }
    };

    fetchUserData();
  }, []);

  const handleLogout = async () => {
    try {
      await AsyncStorage.removeItem('userName');
      await AsyncStorage.removeItem('userType');
      Alert.alert('Logout', 'Você saiu com sucesso!');
      navigation.navigate('login/index'); // Redireciona para a tela de login
    } catch (error) {
      console.error('Error during logout:', error);
    }
  };

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        {userName ? (
          <>
            <Text style={styles.welcomeText}>Bem-vindo, {userName}!</Text>
            <TouchableOpacity
              style={styles.button}
              onPress={() => navigation.navigate('available-orders/index')}
            >
              <Text style={styles.buttonText}>Ver Entregas Disponíveis</Text>
            </TouchableOpacity>

            {/* Botão de logout */}
            <TouchableOpacity
              style={styles.button}
              onPress={handleLogout}
            >
              <Text style={styles.buttonText}>Sair</Text>
            </TouchableOpacity>
          </>
        ) : (
          <Text style={styles.loadingText}>Carregando...</Text>
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
    backgroundColor: '#fff',
  },
  background: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  welcomeText: {
    fontSize: 18,
    color: '#fff',
    marginBottom: 20,
  },
  button: {
    backgroundColor: '#2196F3',
    padding: 15,
    borderRadius: 5,
    alignItems: 'center',
    marginTop: 20,
  },
  buttonText: {
    color: '#fff',
    fontSize: 18,
  },
  loadingText: {
    fontSize: 18,
    color: '#333',
  },
  overlay: {
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    width: '100%',
    padding: 20,
  },
});
