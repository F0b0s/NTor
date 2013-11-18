from Crypto.Hash import SHA as SHA1 
from Crypto.PublicKey import RSA 
from Crypto.Signature import PKCS1_v1_5 

f = open("data.txt", 'r')
data = f.read()

print(data)

# Import Private key for signing: 
key = RSA.importKey("test.pem") 

print(key)

# hash data: 
hash = SHA1.new(data) 

# Create PKCS1 padding object and sign 
pkcs = PKCS1_v1_5.new(key) 
signatureBytes = pkcs.sign(hash)

print(signatureBytes)
