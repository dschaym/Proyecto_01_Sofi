# Proyecto_01 Simulador de Decisiones para Plataforma de Streaming
Este programa es un simulador que permite evaluar contenidos que podrían publicarse en una plataforma de streaming.
El sistema solicita información del contenido (tipo, duración, clasificación, horario y nivel de producción) y aplica las reglas técnicas obligatorias establecidas en el proyecto. Si alguna regla se incumple, el contenido se rechaza inmediatamente.
Los contenidos que sí cumplen todas las reglas técnicas pasan a una clasificación de impacto, y según ese nivel (Bajo, Medio o Alto), el sistema toma una decisión final: Publicar, Publicar con ajustes, Enviar a revisión o Rechazar
El sistema también mantiene estadísticas de la sesión, como cantidad de evaluaciones, rechazados, publicados y el impacto predominante.
Se debe de ingresar al programa para poder ejecutar el simulador y aparecerá un menu que indicará lo que debe de hacer.
El sistema usa:
- switch para el menú
- do-while para mantener activo el programa
- while + TryParse para validar entradas
- if encadenado y anidado para las reglas
- for en la parte visual del sistema
- estadísticas simples sin usar arreglos ni listas
