import React, { useState } from 'react';
import { StyleSheet, View, Text, TextInput, TouchableOpacity, ImageBackground, ScrollView, Alert } from 'react-native';
import { Picker } from '@react-native-picker/picker';
import logo from '../../assets/images/logo.jpg';
import { BASE_URL} from '../../config';

export default function SignupScreen() {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [documentNumber, setDocumentNumber] = useState('');
  const [email, setEmail] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  // Novos campos para o veículo
  const [vehicleBrand, setVehicleBrand] = useState('');
  const [vehicleLicensePlate, setVehicleLicensePlate] = useState('');
  const [vehicleLoadCapacity, setVehicleLoadCapacity] = useState('');
  const [vehicleManufactureYear, setVehicleManufactureYear] = useState('');
  const [vehicleModel, setVehicleModel] = useState('');
  const [vehicleType, setVehicleType] = useState('');

  const [signupSuccess, setSignupSuccess] = useState(false);
  const [userData, setUserData] = useState(null);

  const handleSignup = async () => {
    try {
      const payload = {
          FirstName: firstName,
          LastName: lastName,
          DocumentNumber: documentNumber,
          Email: email,
          PhoneNumber: phoneNumber,
          Username: username,
          Password: password,
          Vehicle: {
            Brand: vehicleBrand,
            LicensePlate: vehicleLicensePlate,
            LoadCapacity: vehicleLoadCapacity,
            ManufactureYear: vehicleManufactureYear,
            Model: vehicleModel,
            VehicleType: 1
          },
      };
  
      console.log('Payload being sent:', payload);
  
      const response = await fetch(`${BASE_URL}/deliveryperson/create`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      });
  
      const data = await response.json();
      if (!response.ok) {
        const errorMessage = data.message || JSON.stringify(data);
        throw new Error(`Erro ao cadastrar usuário: ${errorMessage}`);
      }
  
      console.log('Cadastro realizado com sucesso!');
      setUserData(data);
      setSignupSuccess(true);
    } catch (error) {
      let errorMessage = 'Erro desconhecido';
  
      if (error instanceof Error) {
        errorMessage = error.message;
      } else if (typeof error === 'string') {
        errorMessage = error;
      } else if (error && typeof error === 'object' && 'message' in error) {
        errorMessage = (error as any).message;
      }
  
      console.error('Erro:', errorMessage);
      Alert.alert('Erro', errorMessage);
    }
  };

  const renderSuccessMessage = () => {
    return (
      <View style={styles.successMessage}>
        <Text style={styles.successText}>Cadastro realizado com sucesso!</Text>
        <Text>Nome: {userData.FirstName} {userData.LastName}</Text>
        <Text>Email: {userData.Email}</Text>
        <Text>Telefone: {userData.PhoneNumber}</Text>
      </View>
    );
  };

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        <ScrollView contentContainerStyle={styles.scrollContainer}>
          <Text style={styles.title}>Seus Dados</Text>
          {!signupSuccess ? (
            <>
              <View style={styles.section}>
                <TextInput
                  style={styles.input}
                  placeholder="Nome"
                  placeholderTextColor="#888"
                  value={firstName}
                  onChangeText={setFirstName}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Sobrenome"
                  placeholderTextColor="#888"
                  value={lastName}
                  onChangeText={setLastName}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Número do Documento"
                  placeholderTextColor="#888"
                  value={documentNumber}
                  onChangeText={setDocumentNumber}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Email"
                  placeholderTextColor="#888"
                  value={email}
                  onChangeText={setEmail}
                  keyboardType="email-address"
                />
                <TextInput
                  style={styles.input}
                  placeholder="Telefone"
                  placeholderTextColor="#888"
                  value={phoneNumber}
                  onChangeText={setPhoneNumber}
                  keyboardType="phone-pad"
                />
                <TextInput
                  style={styles.input}
                  placeholder="Nome de Usuário"
                  placeholderTextColor="#888"
                  value={username}
                  onChangeText={setUsername}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Senha"
                  placeholderTextColor="#888"
                  value={password}
                  onChangeText={setPassword}
                  secureTextEntry
                />
              </View>
              <View style={styles.section}>
                <Text style={styles.sectionTitle}>Dados do Veículo</Text>
                <TextInput
                  style={styles.input}
                  placeholder="Marca do Veículo"
                  placeholderTextColor="#888"
                  value={vehicleBrand}
                  onChangeText={setVehicleBrand}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Placa do Veículo"
                  placeholderTextColor="#888"
                  value={vehicleLicensePlate}
                  onChangeText={setVehicleLicensePlate}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Capacidade de Carga"
                  placeholderTextColor="#888"
                  value={vehicleLoadCapacity}
                  onChangeText={setVehicleLoadCapacity}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Ano de Fabricação"
                  placeholderTextColor="#888"
                  value={vehicleManufactureYear}
                  onChangeText={setVehicleManufactureYear}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Modelo do Veículo"
                  placeholderTextColor="#888"
                  value={vehicleModel}
                  onChangeText={setVehicleModel}
                />
                <Picker
                  selectedValue={vehicleType}
                  style={styles.picker}
                  onValueChange={(itemValue) => setVehicleType(itemValue)}
                >
                  <Picker.Item label="Bicicleta" value="Bike" />
                  <Picker.Item label="Carro" value="Car" />
                  <Picker.Item label="Motocicleta" value="Motorcycle" />
                </Picker>
              </View>
              <TouchableOpacity style={styles.button} onPress={handleSignup}>
                <Text style={styles.buttonText}>Cadastrar</Text>
              </TouchableOpacity>
            </>
          ) : renderSuccessMessage()}
        </ScrollView>
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
    width: '100%',
  },
  scrollContainer: {
    padding: 20,
    alignItems: 'center',
  },
  title: {
    fontSize: 32,
    fontWeight: 'bold',
    color: '#fff',
    marginBottom: 16,
  },
  section: {
    width: '100%',
    marginBottom: 20,
  },
  sectionTitle: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#fff',
    marginBottom: 10,
  },
  input: {
    width: '100%',
    backgroundColor: '#fff',
    padding: 10,
    marginBottom: 16,
    borderRadius: 5,
    color: '#000',
  },
  picker: {
    width: '100%',
    backgroundColor: '#fff',
    marginBottom: 16,
  },
  button: {
    backgroundColor: '#2196F3',
    padding: 15,
    borderRadius: 5,
    alignItems: 'center',
    marginTop: 20,
    width: '100%',
  },
  buttonText: {
    color: '#fff',
    fontSize: 18,
    fontWeight: 'bold',
  },
  successMessage: {
    alignItems: 'center',
  },
  successText: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#fff',
    marginBottom: 16,
  },
});
