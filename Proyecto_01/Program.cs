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
    Console.Clear();
    for (int i = 0; i < 23; i++) Console.Write("=");
    Console.WriteLine("\nEVALUACIÓN DE CONTENIDO");
    for (int i = 0; i < 23; i++) Console.Write("=");
    int tipo = LeerTipoContenido();          // 1 Película, 2 Serie, 3 Documental, 4 En vivo
    int duracion = LeerDuracion();           // > 0
    int clasif = LeerClasificacion();        // 1 Todo público, 2 +13, 3 +18
    int hora = LeerHoraProgramada();         // 0-23
    int produccion = LeerNivelProduccion();  // 1 Bajo, 2 Medio, 3 Alto

    // a) Validación técnica (reglas obligatorias)
    string razonTecnica;
    bool okTecnico = ValidacionTecnica(tipo, duracion, clasif, hora, produccion, out razonTecnica);

    string decision;
    string detalleDecision;
    //Si falla cualquier regla obligatoria, rechazar y no segir a impacto
    if (!okTecnico)
    {
        decision = "Rechazar";
        detalleDecision = "Incumple regla obligatoria: " + razonTecnica;
        rechazados++;
        totalEvaluados++;

        for (int i = 0; i < 100; i++) Console.Write("=");
        Console.WriteLine();
        Console.WriteLine($"DECISIÓN: {decision}");
        Console.WriteLine($"DETALLE : {detalleDecision}");
        for (int i = 0; i < 100  ; i++) Console.Write("=");

        return;
    }
    // b) Clasificación de impacto (Bajo/Medio/Alto)
    string impacto = ClasificarImpacto(duracion, hora, produccion);

    // Se contabiliza el impacto para "predominante"
    if (impacto == "Bajo") impactoBajo++;
    else if (impacto == "Medio") impactoMedio++;
    else impactoAlto++;

    // c) Decisión final
    string decisionFinal;
    string detalleFinal;
        if (impacto == "Alto")
        {
            decisionFinal = "Enviar a revisión";
            detalleFinal = "Impacto Alto: producción alta, o duración mayor de 120 minutos o programado entre 20 y 23 horas.";
            enRevision++;
        }
        else
        {
            bool requiereAjuste = RequiereAjusteMenor(tipo, duracion, clasif, hora); // no viola reglas técnicas
            if (requiereAjuste)
            {
                decisionFinal = "Publicar con ajustes";
                detalleFinal = "Cumple reglas; pero requiere modificación menor (puede ajustar horario permitido o duración dentro del rango).";
                publicados++;
            }
            else
            {
                decisionFinal = "Publicar";
                detalleFinal = "Cumple reglas técnicas e impacto " + impacto + ".";
                publicados++;
            }
        }
    totalEvaluados++;
    for (int i = 0; i < 100; i++) Console.Write("=");
    Console.WriteLine();
    Console.WriteLine($"DECISIÓN: {decisionFinal}");
    Console.WriteLine($"DETALLE : {detalleFinal}");
    for (int i = 0; i < 100; i++) Console.Write("=");
}
// ---- Reglas técnicas (usa AND/OR/NOT e if anidado/encadenado) ----
bool ValidacionTecnica(int tipo, int duracion, int clasif, int hora, int produccion, out string razon)
{
    // 1) Clasificación vs horario
    // TP (1): cualquier hora
    // +13 (2): 6–22
    // +18 (3): 22–23 o 0–5
    bool horarioPermitido;
    if (clasif == 1) // Todo público
    {
        horarioPermitido = true;
    }
    else if (clasif == 2) // +13
    {
        horarioPermitido = (hora >= 6 && hora <= 22);            // AND
    }
    else // +18
    {
        horarioPermitido = (hora >= 22 && hora <= 23) || (hora >= 0 && hora <= 5); // OR
    }
    if (!horarioPermitido) // NOT
    {
        razon = "Horario no permitido para la clasificación.";
        return false;
    }
    // 2) Duración por tipo
    // Película: 60–180 | Serie: 20–90 | Documental: 30–120 | En vivo: 30–240
    int min = 0, max = 0;
    if (tipo == 1) { min = 60; max = 180; }
    else if (tipo == 2) { min = 20; max = 90; }
    else if (tipo == 3) { min = 30; max = 120; }
    else                { min = 30; max = 240; }

    if (duracion < min || duracion > max) // OR
    {
        razon = $"Duración fuera de rango para este tipo ({min}-{max} min).";
        return false;
    }

    // 3) Producción
    // Baja (1) solo válida para TP o +13. No permitida para +18.
    bool prodBaja = (produccion == 1);
    bool es18 = (clasif == 3);
    if (prodBaja && es18) // AND
    {
        razon = "Producción baja no permitida para +18.";
        return false;
    }
    razon = "OK";
    return true;
}
// ---- Clasificación de impacto ----
// Alto: producción alta || duración >120 || hora 20–23
// Medio: producción media || 60–120
// Bajo: producción baja y <60
string ClasificarImpacto(int duracion, int hora, int produccion)
{
    bool prodAlta = (produccion == 3);
    bool prodMedia = (produccion == 2);
    bool franjaAlta = (hora >= 20 && hora <= 23); // noche

    if (prodAlta || duracion > 120 || franjaAlta) //
        return "Alto";

    if (prodMedia || (duracion >= 60 && duracion <= 120)) // OR con AND en rango
        return "Medio";

    // Si no fue Alto ni Medio y la producción es baja con <60, queda Bajo
    return "Bajo";
}
// "Publicar con ajustes"
// Cumple técnicamente, pero está al borde de límites
bool RequiereAjusteMenor(int tipo, int duracion, int clasif, int hora)
{
    // Rango del tipo
    int min = 0, max = 0;
    if (tipo == 1) { min = 60; max = 180; } // Pelicula
    else if (tipo == 2) { min = 20; max = 90; } // Serie
    else if (tipo == 3) { min = 30; max = 120; } // Documental
    else                { min = 30; max = 240; } // En vivo

    bool cercaDelMin = (duracion >= min && duracion <= min + 5);
    bool cercaDelMax = (duracion <= max && duracion >= max - 5);

    // Horarios “al límite” según clasificación
    bool tardePara13   = (clasif == 2) && (hora >= 21 && hora <= 22);
    bool madrugadaParaTP = (clasif == 1) && (hora >= 0 && hora <= 5);

    return cercaDelMin || cercaDelMax || tardePara13 || madrugadaParaTP;
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
        Console.Write("Entrada inválida. La hora debe de estar entre 0 y 23. Por favor, intente de nuevo. ");
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
void MostrarReglas()
{
    Console.Clear();
    for (int i = 0; i < 19; i++) Console.Write("=");
    Console.WriteLine("\nREGLAS DEL SISTEMA");
    for (int i = 0; i < 19; i++) Console.Write("=");
    Console.WriteLine();

    Console.WriteLine("\nClasificación y horario:");
    Console.WriteLine("  - Todo público: cualquier hora");
    Console.WriteLine("  - +13: entre 6 y 22 horas");
    Console.WriteLine("  - +18: entre 22 y 5 horas (22–23 o 0–5)");

    Console.WriteLine("\nDuración por tipo:");
    Console.WriteLine("  - Película: 60–180 min");
    Console.WriteLine("  - Serie: 20–90 min");
    Console.WriteLine("  - Documental: 30–120 min");
    Console.WriteLine("  - Evento en vivo: 30–240 min");

    Console.WriteLine("\nProducción:");
    Console.WriteLine("  - Baja: solo válida para Todo público o +13 (no permitida para +18)");
    Console.WriteLine("  - Media/Alta: válida para cualquier clasificación");

    Console.WriteLine("\nDecisiones del sistema:");
    Console.WriteLine("  - Publicar: cumple todas las reglas técnicas y su impacto es Bajo o Medio");
    Console.WriteLine("  - Publicar con ajustes: cumple todas las relaglas, pero requiere modificación menor");
    Console.WriteLine("  - Enviar a revisión: cumple todas las reglas, pero tiene impacto Alto");
    Console.WriteLine("  - Rechazar: si falla una regla técnica");
    Console.WriteLine();
    for (int i = 0; i < 100; i++) Console.Write("=");
}
void MostrarEstadisticas()
{
    Console.Clear();
    for (int i = 0; i < 25; i++) Console.Write("=");
    Console.WriteLine("\nESTADÍSTICAS DE LA SESIÓN");
    for (int i = 0; i < 25; i++) Console.Write("=");
    Console.WriteLine();
    Console.WriteLine($"Total: {totalEvaluados}");
    Console.WriteLine($"Publicados: {publicados}");
    Console.WriteLine($"Rechazados: {rechazados}");
    Console.WriteLine($"En Revisión: {enRevision}");
    string impactoPredominante = "Ninguno";

    if (impactoAlto >= impactoMedio && impactoAlto >= impactoBajo && impactoAlto > 0)
        impactoPredominante = "Alto";
    else if (impactoMedio >= impactoAlto && impactoMedio >= impactoBajo && impactoMedio > 0)
        impactoPredominante = "Medio";
    else if (impactoBajo >= impactoAlto && impactoBajo >= impactoMedio && impactoBajo > 0)
        impactoPredominante = "Bajo";

    Console.WriteLine($"Impacto predominante: {impactoPredominante}");
    double porcentajeAprobacion = 0.0;
    if (totalEvaluados > 0)
        porcentajeAprobacion = (publicados * 100.0) / totalEvaluados;

    Console.WriteLine($"Porcentaje de aprobación: {porcentajeAprobacion:0.0}%");
    for (int i = 0; i < 50; i++) Console.Write("=");
}
void ReiniciarEstadisticas()
{
    totalEvaluados = 0;
    publicados = 0;
    rechazados = 0;
    enRevision = 0;
    impactoBajo = 0;
    impactoMedio = 0;
    impactoAlto = 0;
    Console.WriteLine("\nESTADÍSTICAS REINICIADAS...");
}
void Pausa()
{
    Console.WriteLine("\nPresione una tecla para continuar...");
    Console.ReadKey();
}