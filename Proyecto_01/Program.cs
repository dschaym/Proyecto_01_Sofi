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

    int tipo = LeerTipoContenido();          // 1-4
    int duracion = LeerDuracion();           // >0
    int clasif = LeerClasificacion();        // 1-3
    int hora = LeerHoraProgramada();         // 0-23
    int produccion = LeerNivelProduccion();  // 1-3

    Console.WriteLine($"\nLeído -> Tipo:{tipo}, Duración:{duracion}, Clasif:{clasif}, Hora:{hora}, Prod:{produccion}");
    Console.WriteLine("(En la siguiente parte aplico reglas técnicas, impacto y decisión)");
}
int LeerTipoContenido()
{
    Console.WriteLine("\nTipo de contenido:");
    Console.WriteLine("  1.) Película");
    Console.WriteLine("  2.) Serie");
    Console.WriteLine("  3.) Documental");
    Console.WriteLine("  4.) Evento en vivo");
    Console.Write("Seleccione (1-4): ");

    int valor;
    while (!int.TryParse(Console.ReadLine(), out valor) || valor < 1 || valor > 4)
        Console.Write("Inválido. Escriba 1-4: ");
    return valor;
}
int LeerDuracion()
{
    Console.Write("\nDuración (minutos): ");
    int valor;
    while (!int.TryParse(Console.ReadLine(), out valor) || valor <= 0)
        Console.Write("Inválido. Ingrese minutos positivos: ");
    return valor;
}
int LeerClasificacion()
{
    Console.WriteLine("\nClasificación:");
    Console.WriteLine("  1.) Todo público");
    Console.WriteLine("  2.) +13");
    Console.WriteLine("  3.) +18");
    Console.Write("Seleccione (1-3): ");

    int valor;
    while (!int.TryParse(Console.ReadLine(), out valor) || valor < 1 || valor > 3)
        Console.Write("Inválido. Escriba 1-3: ");
    return valor;
}
int LeerHoraProgramada()
{
    Console.Write("\nHora programada (0-23): ");
    int valor;
    while (!int.TryParse(Console.ReadLine(), out valor) || valor < 0 || valor > 23)
        Console.Write("Inválido. Ingrese 0-23: ");
    return valor;
}
int LeerNivelProduccion()
{
    Console.WriteLine("\nNivel de producción:");
    Console.WriteLine("  1.) Bajo");
    Console.WriteLine("  2.) Medio");
    Console.WriteLine("  3.) Alto");
    Console.Write("Seleccione (1-3): ");

    int valor;
    while (!int.TryParse(Console.ReadLine(), out valor) || valor < 1 || valor > 3)
        Console.Write("Inválido. Escriba 1-3: ");
    return valor;
}
// Aquí ya podría detallar las reglas técnicas, de impacto y la desicion final :) seguiré mañana
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