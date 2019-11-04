# Lab2_Cifrado
Ricardo Chian 1103916 &amp; Pablo García 1174216

Se realizó un commit fuera tiempo, pero esos cambios solucionaron el problema de precisión en los números que se describió abajo en el último commit. Los nuevos cambios que se hicieron fueron: el uso de tipo de dato en lugar de double se cambió a BigInteger mandando a llamar (Using System.Numerics de .NET) este tipo de dato permite usar números grandes sin restricción alguna, gracias a eso se logró solucionar el problema de las aproximaciones y pérdida de valores. Otro cambio que se realizó fueron en una vista ya que en el último commit se pedia números pequeños, en cambio ahora se puede ingresar de 17 en adelante. Y por último una pequeña mejora al método que calcula "E", antes calculaba 10 posibles y escogía uno de esos de manera aleatoria, ahora selecciona de igual manera uno aleatorio pero previo a seleccionarlo calcula cual de todas las posibilidades genera el "d" más pequeño, por lo que "e" y "d" estarán en un rango entre 1 y 50, para que no existan potencias mayores de 200 y que no ocurra algún tipo de error, ya que en ocasiones mientras más pequeño fuera "e" más grande era "d". En conclusión no hubo cambios de cógido grande mas que cambiar el tipo de dato de double a BigInteger y la optimizacion que se hizo en 5 líneas aproximadamente.

Este era el problema de la entrega a tiempo
----------------------------------------------------------------------------------------------------------------------------------------
La entrega de RSA funciona y no existe algún tipo de error de que alguna operación desborde el tipo de dato, se usó para las operaciones double porque es el tipo de dato que acepta valores muy grandes (hasta 1.7x10^308) en comparacion al tipo "decimal" o "long" que son los otros tipo de datos que aceptan números grandes, pero no tan grandes como los del double.

El problema está que aunque se escojan p=11 q =13 (al escoger estos valores n se restringe a 143) va a generar una llave pública 221,5 (como generaliacion ya que nuestro algoritmo calcula llaves diferentes porque el valor de E escoge uno entre 3 posibles que se calculan) y una privada de 221,77 con la pública no habría ningún problema ya que 5 es un exponente pequeño para el programa entonces la fórmula
C=N^e mod n se calcula de manera precisa.

El problema sería en el descifrado que al hacer N = C^d mod d, el cálculo de la potencia d que sería el exponenete es 77 y es un número demasiado grande (por ejemplo si lee una s = 115 y se hace 115^77 es un número nuy grande) que el double sí va a soportar, el problema es que cuando trabaja con números demasiado grandes los empieza a trabajar exponencialmente y se empiezan a perder números, por lo que al descifrar nunca llegará a ser el caracter original que se tenía.

Y si se prueba con números p y q más pequeños nos topamos con la restricción de n, entonces el programa se puede probar con números primos mayores 6 y menores a 35.

No van a existir errores de códificación que dentengan el programa, pero aún así no va a descifrar correctamente porque la perdida de exactitud de un número demasiado grande afecta en el resultado.
