USE geekstra_CG_ctrlgral
--select * from ca_opciones where controlador='ControlFinanciero'
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
VALUES  ( 819 ,
          'ControlFinanciero' ,
          'ValidarMes' ,
          'Tesoreria' ,
          NULL ,
          0 ,
          0 ,
          0 ,
          0 ,
          200 ,
          NULL ,
          NULL ,
          1 ,
          GETDATE()
        )