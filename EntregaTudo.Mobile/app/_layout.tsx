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
import ConfirmOrderScreen from './confirm-order';
import Login from './login';

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
          <Stack.Screen name="delivery-details/index" component={DeliveryDetailsScreen} options={{ title: 'Calcular Pedido' }} />
          <Stack.Screen name="confirm-order/index" component={ConfirmOrderScreen} options={{ title: 'Confirmar Pedido' }} />
        </Stack.Navigator>
    </ThemeProvider>
  );
}
