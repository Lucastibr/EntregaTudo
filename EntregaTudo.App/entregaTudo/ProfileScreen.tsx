import React, { useEffect, useState } from 'react';
import { StyleSheet, View, Text, Button, ImageBackground } from 'react-native';
import AsyncStorage from '@react-native-async-storage/async-storage';
import axios from 'axios';
import { StackNavigationProp } from '@react-navigation/stack';
import { RootStackParamList } from './types';
import logo from './images/logo.jpg';

type ProfileScreenNavigationProp = StackNavigationProp<RootStackParamList, 'Profile'>;

type Props = {
  navigation: ProfileScreenNavigationProp;
};

const ProfileScreen: React.FC<Props> = ({ navigation }) => {
  const [userInfo, setUserInfo] = useState<any>(null);

  useEffect(() => {
    const fetchUserInfo = async () => {
      try {
        const token = await AsyncStorage.getItem('token');
        if (token) {
          const response = await axios.get('http://10.0.2.2:5240/login/userinfo', {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });
          setUserInfo(response.data);
        } else {
          navigation.navigate('Login');
        }
      } catch (error) {
        console.error('Failed to fetch user info', error);
        navigation.navigate('Login');
      }
    };

    fetchUserInfo();
  }, [navigation]);

  if (!userInfo) {
    return (
      <ImageBackground source={logo} style={styles.background}>
        <View style={styles.overlay}>
          <Text style={styles.loadingText}>Carregando...</Text>
        </View>
      </ImageBackground>
    );
  }

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        <Text style={styles.title}>Perfil do Usuário</Text>
        <Text style={styles.info}>Nome: {userInfo.firstName} {userInfo.lastName}</Text>
        <Text style={styles.info}>Email: {userInfo.email}</Text>
        <Text style={styles.info}>Número do Documento: {userInfo.documentNumber}</Text>
        <Text style={styles.info}>Telefone: {userInfo.phoneNumber}</Text>
        <Text style={styles.info}>Tipo de Pessoa: {userInfo.personType}</Text>
        <Button
          title="Logout"
          onPress={async () => {
            await AsyncStorage.removeItem('token');
            navigation.navigate('Login');
          }}
        />
      </View>
    </ImageBackground>
  );
};

export default ProfileScreen;

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
  info: {
    fontSize: 18,
    color: '#ccc',
    textAlign: 'center',
    marginBottom: 16,
  },
  loadingText: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#fff',
  },
});
