import { DarkTheme, DefaultTheme, ThemeProvider } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { NavigationContainer } from '@react-navigation/native';
import { useFonts } from 'expo-font';
import * as SplashScreen from 'expo-splash-screen';
import { useEffect } from 'react';
import 'react-native-reanimated';
import { useColorScheme } from '@/hooks/useColorScheme';

// Prevent the splash screen from auto-hiding before asset loading is complete.
SplashScreen.preventAutoHideAsync();

const Stack = createStackNavigator();

import HomeScreen from './home-screen';
import SelectItemScreen from './select-item';
import EnterAddressScreen from './enter-address';
import DeliveryDetailsScreen from './delivery-details';
import OrderCustomers from './orders-customers';
import Login from './login';
import DeliveryWelcomeScreen from './delivery-person-home-screen';
import AvailableOrders from './available-orders';
import DeliveryOrderDetails from './delivery-order-details';
import SignupScreen from './signup-screen-customer';
import SignupScreenDeliveryPerson from './signup-screen-delivery-person';

export default function RootLayout() {
  const colorScheme = useColorScheme();
  const [loaded] = useFonts({
    SpaceMono: require('../assets/fonts/SpaceMono-Regular.ttf'),
  });

  useEffect(() => {
    if (loaded) {
      SplashScreen.hideAsync();
    }
  }, [loaded]);

  if (!loaded) {
    return null;
  }

  return (
    <ThemeProvider value={colorScheme === 'dark' ? DarkTheme : DefaultTheme}>
        <Stack.Navigator>
          <Stack.Screen name="home-screen/index" component={HomeScreen} options={{ title: 'Home' }} />
          <Stack.Screen name="login/index" component={Login} options={{ title: 'Login' }} />
          <Stack.Screen name="select-item/index" component={SelectItemScreen} options={{ title: 'Selecione os Itens' }} />
          <Stack.Screen name="enter-address/index" component={EnterAddressScreen} options={{ title: 'Digite o Endereço' }} />
          <Stack.Screen name="delivery-details/index" component={DeliveryDetailsScreen} options={{ title: 'Detalhes do Pedido' }} />
          <Stack.Screen name="orders-customers/index" component={OrderCustomers} options={{ title: 'Meus Pedidos' }} />
          <Stack.Screen name="delivery-person-home-screen/index" component={DeliveryWelcomeScreen} options={{ title: 'Bem vindo, Entregador!' }} />
          <Stack.Screen name="available-orders/index" component={AvailableOrders} options={{ title: 'Selecione a Entrega!' }} />
          <Stack.Screen name="delivery-order-details/index" component={DeliveryOrderDetails} options={{ title: 'Entrega em Andamento' }} />
          <Stack.Screen name="signup-screen-customer/index" component={SignupScreen} options={{ title: 'Cadastrar Usuário' }} />
          <Stack.Screen name="signup-screen-delivery-person/index" component={SignupScreenDeliveryPerson} options={{ title: 'Cadastrar Entregador' }} />
        </Stack.Navigator>
    </ThemeProvider>
  );
}
