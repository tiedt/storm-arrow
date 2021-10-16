using System.Net.NetworkInformation;
using UnityEngine;

public static class ServicosUtils{
    
    public static string RetornaMelhorEnderecoMac() {
        const int MIN_MAC_ADDR_LENGTH = 12;
        string macAddress = string.Empty;
        long maxSpeed = -1;

        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()){
//            Debug.Log("Found MAC Address: " + nic.GetPhysicalAddress() +
//                      " Type: " + nic.NetworkInterfaceType);

            string tempMac = nic.GetPhysicalAddress().ToString();
            if (nic.Speed > maxSpeed &&
                !string.IsNullOrEmpty(tempMac) &&
                tempMac.Length >= MIN_MAC_ADDR_LENGTH){
//                Debug.Log("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                maxSpeed = nic.Speed;
                macAddress = tempMac;
            }
        }

        /*
         * Formata o endereço mac encontrado
         */
        if(!string.IsNullOrEmpty(macAddress)) {
            for(int i = 5; i >= 1; i--) {
                macAddress = macAddress.Insert(i * 2 , "-");
            }
        }
        return macAddress;
    }

}
