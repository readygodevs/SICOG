# README #
Sistema de contabilidad

This README would normally document whatever steps are necessary to get your application up and running.

### What is this repository for? ###

* Quick summary
* Version
* [Learn Markdown](https://bitbucket.org/tutorials/markdowndemo)

### How do I get set up? ###

* Summary of set up
* Configuration
* Dependencies
* Database configuration
* How to run tests
* Deployment instructions

### Contribution guidelines ###

* Writing tests
* Code review
* Other guidelines

### Who do I talk to? ###

* Repo owner or admin
* Other community or team contact

Recibo de ingresos:

-> Seleccionar caja receptora
-> Fecha de captura
-> Fecha de cancelación
-> Fecha de recaudación (Impreso)
-> Usuario Sistema
-> Referencias de Polizas de Devengado y Recuadado y sus cancelaciones
-> Foliador (Último Folio de Ingresos)
-> Nombre de contribuyente (Ca_Personas)
-> Observaciones
-> Impreso
-> Estatus
-> Cuenta Bancaria

/***************Detalles****************/

-> CRI (Solo Abonos) [Obtener del Ma_PresupuestoIng]
-> Cuentas Balance (Cargos) [Obtener del Ma_PresupuestoIng]
-> Antes de salir, solicitar cuenta bancaria (Se guarda en el maestro no en los detalles).
-> Tipo y folio de Movimiento Bancario (Pendiente).


/************Reglas Negocios**********/

-> Edutar Recibo
	-> Solo si no esta impreso se puede editar. 

-> Imprimir Recibo 
	-> Pedir fecha de recaudación y mandar llamar procedimientos para generar la poliza de orden recaudado y devengado. 
-> Cancelar Recibo
	-> Si no esta impreso se puede cancelar y se cambia el estatus a cancelado.
	-> Si esta impreso y no tiene poliza de orden de devengado hacer el paso anterior. 
	-> Si si esta impreso y si tiene poiliza de orden de devengado, pedir fecha de cancelción y mandar llamar procedimiento de cancelacion de polizas de orden devengado y recaudado
		-> Mostrar polizas de cancelado en el maestro.