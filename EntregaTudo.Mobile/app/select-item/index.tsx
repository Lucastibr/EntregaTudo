import React, { useState } from 'react';
import { View, Text, TextInput, Alert, StyleSheet, ImageBackground, TouchableOpacity } from 'react-native';
import { StackNavigationProp } from '@react-navigation/stack';
import { RootStackParamList } from '../../components/types';
import logo from '../../assets/images/logo.jpg';

type SelectItemScreenNavigationProp = StackNavigationProp<RootStackParamList, 'select-item/index'>;

type Props = {
  navigation: SelectItemScreenNavigationProp;
};

const SelectItemScreen: React.FC<Props> = ({ navigation }) => {
  const [item, setItem] = useState<string>('');

  const handleNext = () => {
    if (item.trim()) {
      navigation.navigate('enter-address/index', { item });
    } else {
      Alert.alert('Por favor, insira o item para entrega');
    }
  };

  return (
    <ImageBackground source={logo} style={styles.background}>
      <View style={styles.overlay}>
        <Text style={styles.label}>O que você quer entregar?</Text>
        <TextInput
          style={styles.input}
          value={item}
          onChangeText={setItem}
          placeholder="Ex: Documento"
          placeholderTextColor="#ccc"
        />
        <TouchableOpacity style={styles.button} onPress={handleNext}>
          <Text style={styles.buttonText}>Próximo</Text>
        </TouchableOpacity>
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
  overlay: {
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    width: '100%',
    padding: 20,
  },
  label: {
    fontSize: 18,
    color: '#fff',
    marginBottom: 8,
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
  button: {
    width: '100%',
    paddingVertical: 10,
    backgroundColor: '#2196F3',
    borderRadius: 5,
    alignItems: 'center',
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold',
  },
});

export default SelectItemScreen;
