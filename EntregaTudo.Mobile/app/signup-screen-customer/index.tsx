import React, { useState } from 'react';
import { StyleSheet, View, Text, TouchableOpacity, ImageBackground, Modal, KeyboardAvoidingView, Platform } from 'react-native';
import { TextInputMask } from 'react-native-masked-text';
import { useNavigation } from '@react-navigation/native'; // Importa o hook de navegação
import logo from '../../assets/images/logo.jpg';
import { BASE_URL } from '../../config';
import { Ionicons } from '@expo/vector-icons';

export default function SignupScreen() {
  const navigation = useNavigation(); // Inicializa a navegação

  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [documentNumber, setDocumentNumber] = useState('');
  const [email, setEmail] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [signupSuccess, setSignupSuccess] = useState(false);
  const [userData, setUserData] = useState(null);
  const [showValidationError, setShowValidationError] = useState(false);
  const [isPasswordVisible, setIsPasswordVisible] = useState(false);

  const handleSignup = () => {
    if (!firstName || !lastName || !documentNumber || !email || !phoneNumber || !username || !password) {
      setShowValidationError(true);
      return;
    }

    fetch(`${BASE_URL}/customer/create`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        FirstName: firstName,
        LastName: lastName,
        DocumentNumber: documentNumber,
        Email: email,
        PhoneNumber: phoneNumber,
        username: username,
        password: password,
      }),
    })
      .then(response => {
        if (!response.ok) {
          throw new Error('Erro ao cadastrar usuário');
        }
        return response.json();
      })
      .then(data => {
        console.log('Cadastro realizado com sucesso!');
        setUserData(data);
        setSignupSuccess(true);
      })
      .catch((error) => {
        console.error('Erro:', error);
      });
  };

  return (
    <KeyboardAvoidingView
      style={styles.container}
      behavior={Platform.OS === 'ios' ? 'padding' : undefined}
    >
      <ImageBackground source={logo} style={styles.background}>
        <View style={styles.overlay}>
          <Text style={styles.title}>Cadastro de Usuário</Text>
          {!signupSuccess ? (
            <>
              <TextInputMask
                type={'custom'}
                options={{
                  mask: '**************************'
                }}
                style={styles.input}
                placeholder="Nome"
                placeholderTextColor="#888"
                value={firstName}
                onChangeText={setFirstName}
              />
              <TextInputMask
                type={'custom'}
                options={{
                  mask: '**************************'
                }}
                style={styles.input}
                placeholder="Sobrenome"
                placeholderTextColor="#888"
                value={lastName}
                onChangeText={setLastName}
              />
              <TextInputMask
                type={'cpf'}
                options={{
                  mask: '999.999.999-99',
                  getMask: (value) => value.length > 11 ? '99.999.999/9999-99' : '999.999.999-99'
                }}
                value={documentNumber}
                onChangeText={setDocumentNumber}
                style={styles.input}
                placeholder="CPF ou CNPJ"
                placeholderTextColor="#888"
                keyboardType="numeric"
              />
              <TextInputMask
                type={'custom'}
                options={{
                  mask: '**************************'
                }}
                style={styles.input}
                placeholder="Email"
                placeholderTextColor="#888"
                value={email}
                onChangeText={setEmail}
                keyboardType="email-address"
              />
              <TextInputMask
                type={'cel-phone'}
                options={{
                  mask: '(99) 99999-9999',
                  maskType: 'BRL'
                }}
                style={styles.input}
                placeholder="Telefone"
                placeholderTextColor="#888"
                value={phoneNumber}
                onChangeText={setPhoneNumber}
                keyboardType="phone-pad"
              />
              <TextInputMask
                type={'custom'}
                options={{
                  mask: '**************************'
                }}
                style={styles.input}
                placeholder="Nome de Usuário"
                placeholderTextColor="#888"
                value={username}
                onChangeText={setUsername}
              />
              <View style={styles.passwordContainer}>
                <TextInputMask
                  type={'custom'}
                  options={{
                    mask: '**************************'
                  }}
                  style={styles.passwordInput}
                  placeholder="Senha"
                  placeholderTextColor="#888"
                  value={password}
                  onChangeText={setPassword}
                  secureTextEntry={!isPasswordVisible}
                />
                <TouchableOpacity onPress={() => setIsPasswordVisible(!isPasswordVisible)}>
                  <Ionicons
                    name={isPasswordVisible ? 'eye' : 'eye-off'}
                    size={24}
                    color="#888"
                  />
                </TouchableOpacity>
              </View>
              <TouchableOpacity style={styles.button} onPress={handleSignup}>
                <Text style={styles.buttonText}>Cadastrar</Text>
              </TouchableOpacity>
            </>
          ) : (
            <>
              <Text style={styles.successText}>Cadastro realizado com sucesso!</Text>
              <TouchableOpacity
                style={styles.button}
                onPress={() => navigation.navigate('login/index')} // Navega para a tela de login
              >
                <Text style={styles.buttonText}>Ir para Login</Text>
              </TouchableOpacity>
            </>
          )}
        </View>

        <Modal
          visible={showValidationError}
          transparent={true}
          animationType="slide"
          onRequestClose={() => setShowValidationError(false)}
        >
          <View style={styles.modalContainer}>
            <View style={styles.modalContent}>
              <Text style={styles.modalText}>Erro</Text>
              <Text style={styles.modalMessage}>Por favor, preencha todos os campos.</Text>
              <TouchableOpacity style={styles.closeButton} onPress={() => setShowValidationError(false)}>
                <Text style={styles.closeButtonText}>Fechar</Text>
              </TouchableOpacity>
            </View>
          </View>
        </Modal>
      </ImageBackground>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
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
  input: {
    width: '100%',
    padding: 10,
    borderWidth: 1,
    borderColor: '#ccc',
    marginBottom: 20,
    backgroundColor: '#fff',
    color: '#000',
    borderRadius: 5,
  },
  passwordContainer: {
    width: '100%',
    flexDirection: 'row',
    alignItems: 'center',
    borderWidth: 1,
    borderColor: '#ccc',
    borderRadius: 5,
    marginBottom: 20,
    paddingHorizontal: 10,
    backgroundColor: '#fff',
  },
  passwordInput: {
    flex: 1,
    padding: 10,
    color: '#000',
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
  successText: {
    fontSize: 20,
    color: '#fff',
    fontWeight: 'bold',
    marginTop: 20,
    textAlign: 'center',
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
  },
  modalContent: {
    backgroundColor: '#fff',
    padding: 20,
    borderRadius: 8,
    alignItems: 'center',
    width: '80%',
  },
  modalText: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#ff4444',
    marginBottom: 10,
  },
  modalMessage: {
    fontSize: 16,
    color: '#333',
    textAlign: 'center',
    marginBottom: 20,
  },
  closeButton: {
    backgroundColor: '#2196F3',
    padding: 10,
    borderRadius: 5,
    width: '100%',
    alignItems: 'center',
  },
  closeButtonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold',
  },
});
