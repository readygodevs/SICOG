USE geekstra_CG_ctrlgral
--USE BD_CtrlGral
select * from ca_opciones where accion='GraficaDisponibilidad'
--DELETE FROM ca_opciones WHERE idopcion=1381
--UPDATE ca_opciones SET orden=7 WHERE idopcion=1380
INSERT  INTO ca_opciones
        ( idopcionp ,
          controlador ,
          accion ,
          sistema ,
          descripcion ,
          barra ,
          barrapadre ,
          menu ,
          menupadre ,
          submenuwidth ,
          mostrar ,
          orden ,
          usuact ,
          fact
        )
VALUES  ( NULL ,
          'Account' ,
          'GraficaDisponibilidad' ,
          'Tesoreria' ,
          NULL ,
          0 ,
          0 ,
          0 ,
          735 ,
          200 ,
          NULL ,
          NULL ,
          1 ,
          GETDATE()
        )