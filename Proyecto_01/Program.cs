int totalEvaluados = 0;
int publicados = 0;
int rechazados = 0;
int enRevision = 0;

int impactoBajo = 0;
int impactoMedio = 0;
int impactoAlto = 0;

int opcion;
do
{
    Console.Clear();
    opcion = MostrarMenu();

    switch (opcion)
    {
        case 1:
            EvaluarContenido();
            Pausa();
            break;

        case 2:
            MostrarReglas(); 
            Pausa();
            break;

        case 3:
            MostrarEstadisticas();
            Pausa();
            break;

        case 4:
            ReiniciarEstadisticas();
            Pausa();
            break;

        case 5:
            Console.WriteLine("\nSaliendo... Resumen final:");
            MostrarEstadisticas();
            break;

        default:
            Console.WriteLine("Opción no válida.");
            Pausa();
            break;
    }

} while (opcion != 5);
int MostrarMenu()
{
    Console.WriteLine("SIMULADOR DE DECISIONES PLATAFORMA DE STREAMING");
    Console.WriteLine("1. Evaluar nuevo contenido");
    Console.WriteLine("2. Mostrar reglas del sistema");
    Console.WriteLine("3. Mostrar estadísticas de la sesión");
    Console.WriteLine("4. Reiniciar estadísticas");
    Console.WriteLine("5. Salir");
    Console.Write("Seleccione una opción (1-5): ");

    int valor;
    while (!int.TryParse(Console.ReadLine(), out valor) || valor < 1 || valor > 5)
        Console.Write("Entrada inválida. Ingrese 1-5: ");
    return valor;
}

void EvaluarContenido()
{
    Console.WriteLine("\nEVALUACIÓN DE CONTENIDO");
}

void MostrarReglas()
{
    Console.WriteLine("\nREGLAS DEL SISTEMA");
}

void MostrarEstadisticas()
{
    Console.WriteLine("\nESTADÍSTICAS DE LA SESIÓN");
    Console.WriteLine($"Total: {totalEvaluados}, Publicados: {publicados}, Rechazados: {rechazados}, En revisión: {enRevision}");
}

void ReiniciarEstadisticas()
{
    totalEvaluados = publicados = rechazados = enRevision = 0;
    impactoBajo = impactoMedio = impactoAlto = 0;
    Console.WriteLine("\nESTRATEGÍAS REINICIADAS...");
}

void Pausa()
{
    Console.WriteLine("\nPresione una tecla para continuar...");
    Console.ReadKey();
}