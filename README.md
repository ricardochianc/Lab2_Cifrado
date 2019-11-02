# Lab2_Cifrado
Ricardo Chian 1103916 &amp; Pablo García 1174216

La entrega de RSA funciona y no existe algún tipo de error de que alguna operación desborde el tipo de dato, se usó para las operaciones double porque es el tipo de dato que acepta valores muy grandes (hasta 1.7x10^308) en comparacion al tipo "decimal" o "long" que son los otros tipo de datos que aceptan números grandes, pero no tan grandes como los del double.

El problema está que aunque se escojan p=11 q =13 (al escoger estos valores n se restringe a 143) va a generar una llave pública 221,5 (como generaliacion ya que nuestro algoritmo calcula llaves diferentes porque el valor de E escoge uno entre 3 posibles que se calculan) y una privada de 221,77 con la pública no habría ningún problema ya que 5 es un exponente pequeño para el programa entonces la fórmula
C=N^e mod n se calcula de manera precisa.

El problema sería en el descifrado que al hacer N = C^d mod d, el cálculo de la potencia d que sería el exponenete es 77 y es un número demasiado grande (por ejemplo si lee una s = 115 y se hace 115^77 es un número nuy grande) que el double sí va a soportar, el problema es que cuando trabaja con números demasiado grandes los empieza a trabajar exponencialmente y se empiezan a perder números, por lo que al descifrar nunca llegará a ser el caracter original que se tenía.

Y si se prueba con números p y q más pequeños nos topamos con la restricción de n, entonces el programa se puede probar con números primos mayores 6 y menores a 35.

No van a existir errores de códificación que dentengan el programa, pero aún así no va a descifrar correctamente porque la perdida de exactitud de un número demasiado grande afecta en el resultado.
