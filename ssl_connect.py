import socket, ssl, pprint

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
ssl_sock = ssl.wrap_socket(s)
ssl_sock.connect(('31.19.226.119', 443))
ssl_sock.do_handshake()

print(repr((ssl_sock.cipher())))
print(ssl_sock.cipher())
print(ssl_sock.getpeercert())