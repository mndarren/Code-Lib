:: Ref: https://www.c-sharpcorner.com/blogs/asp-net-core-encrypt-and-decrypt-public-key-and-private-key
:: 1. Download and install Win64 OpenSSL v1.1.1j from https://slproweb.com/products/Win32OpenSSL.html
:: 2. Set openssl config
cd C:\Program Files\OpenSSL-Win64\bin
set OPENSSL_CONF=C:\Program Files\OpenSSL-Win64\bin\openssl.cfg
openssl.exe version
:: Generate private key. Just hit enter again and again in the output window
openssl.exe req -x509 -nodes -days 3650 -newkey rsa:1024 -keyout privatekey.pem -out mycert.pem
:: Generate public key
openssl.exe rsa -in privatekey.pem -pubout -out publickey.pem
:: Generate certificate & create a password
openssl.exe pkcs12 -export -out mycertprivatekey.pfx -in mycert.pem -inkey privatekey.pem -name "my certificate"
:: mycert.pem file is the Public Key; mycertprivatekey is Private Key
:: The following is the related C# code to show how to use the certificate key
::public static string EncryptUsingCertificate(string data) {  
::    try {  
::        byte[] byteData = Encoding.UTF8.GetBytes(data);  
::        string path = Path.Combine(_hostEnvironment.WebRootPath, "mycert.pem");  
::        var collection = new X509Certificate2Collection();  
::        collection.Import(path);  
::        var certificate = collection[0];  
::        var output = "";  
::        using(RSA csp = (RSA) certificate.PublicKey.Key) {  
::            byte[] bytesEncrypted = csp.Encrypt(byteData, RSAEncryptionPadding.OaepSHA1);  
::            output = Convert.ToBase64String(bytesEncrypted);  
::        }  
::        return output;  
::    } catch (Exception ex) {  
::        return "";  
::    }  
::}  
::public static string DecryptUsingCertificate(string data) {  
::    try {  
::        byte[] byteData = Convert.FromBase64String(data);  
::        string path = Path.Combine(_hostEnvironment.WebRootPath, "mycertprivatekey.pfx");  
::        var Password = "123"; //Note This Password is That Password That We Have Put On Generate Keys  
::        var collection = new X509Certificate2Collection();  
::        collection.Import(System.IO.File.ReadAllBytes(path), Password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);  
::        X509Certificate2 certificate = new X509Certificate2();  
::        certificate = collection[0];  
::        foreach(var cert in collection) {  
::            if (cert.FriendlyName.Contains("my certificate")) {  
::                certificate = cert;  
::            }  
::        }  
::        if (certificate.HasPrivateKey) {  
::            RSA csp = (RSA) certificate.PrivateKey;  
::            var privateKey = certificate.PrivateKey as RSACryptoServiceProvider;  
::            var keys = Encoding.UTF8.GetString(csp.Decrypt(byteData, RSAEncryptionPadding.OaepSHA1));  
::            return keys;  
::        }  
::    } catch (Exception ex) {}  
::    return null;  
::}  