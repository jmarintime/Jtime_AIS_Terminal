   M S B A M L   `   `   `  ���� D  @AISServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:AISServer	AISServer      AISServer.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:AISServer     -     � Title$  	AISServer��$��568.518��$��456.297�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � 	Resources  �� 
TargetType   ButtonStyle �� 	Resources �� Triggers P� Property P� Value	 �� Property
 �� Value �� 
TargetType ��Name��%  &	        �� " �� ���� '��       " �� P�  � {� .  . ��� 	 �@J�Opacity$	
 0.5Q�.��� 	 �@�OverridesDefaultStyle
 . �� 	 � ��
 �� " ��.R�� #��z��.+��� $ MyContentPresenter��#��z��$��Center=�$��Center=�.��� ���	#FF070925.+��� -    ��Name$ 	txtLoding��$"��프로그램 Loading 중....��$
��Left=�$��	20,10,0,0q�$	��Top=�
��� E��$��20(�$
��Bold"��� -    ��Name$ tabMainControl��$
��Left=�$	��474��$��	20,47,0,0q�$	��Top=�$	��408�����	#FF070925���	#FF3195D3$��5q�.@��� $��연결 설정�����	#FF3195D3��Z� .��� ��Z� .+��� $��네트워크 설정��$
��Left=�$��	10,12,0,0q�$	��Top=�
�������$��20(��� $��11,42,-1,305q�.+��� $��
ServerIP :��$
��Left=�$	��Top=�$��15(�
������� �� Left$ 23��
 �� Top$ 15���� -    ��Name$ txtServerIP��$��http://127.0.0.1��$	 110��$ 11��$	��170��$��26��$��15(��� $��ServerPort :��$
��Left=�$	��Top=�$��15(�
�������$ 7��$ 55���� -   $ txtServerPort��$
��5000��$	 110��$ 55��$	��170��$��26��$��15(��� -    ��Name$ btnStartServer��$
��Left=�$	��Top=�$��87��$��41��$	 284��$ 95��$��179,142,0,0q�#��[  ��Z� ��Z� .���� $��Resources\btn_connect.png���� -   $ btnStopServer��$
��Left=�$	��Top=�$��87��$��41��$	 284��$ 95��$��296,142,0,0q�{�.  #��[  ��Z� ��Z� .���� $��Resources\btn_stop.png���� $��Serial Port 설정��$
��Left=�$��
10,196,0,0q�$	��Top=�
�������$��20(��� $��10,233,11,125q���Z� .+��� $��Port :��$��Right=�$	��Top=�$��15(�
�������$��31��$��50��$ 42��$ 10���� -    ��Name$ cmbPort��$	 108��$ 19��$	��170���� $��
BaudRate :��$
��Left=�$	��Top=�$��15(�
�������$ 9��$ 41���� -	   $ cmbBaudRate��$	 108��$ 54��$	��170���� -
   $ btnConnectSerialPort��$
��Left=�$	��Top=�$��87��$��41��$	 284��$ 95��$��179,329,0,0q�#��[  ��Z� ��Z� .���� $��Resources\btn_connect.png���� -   $ btnDisconnectSerialPort��$
��Left=�$	��Top=�$��87��$��41��$	 284��$ 95��$��296,329,0,0q�{�.  #��[  ��Z� ��Z� .���� $��Resources\btn_stop.png���� -   $ tbxReceivedMessage��$
��Left=�$��
11,388,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$��32��$	��372��
��������� $��	DB 연결�����	#FF3195D3��Z� .��� ��Z� .+��� $��데이터베이스 설정��$
��Left=�$��	11,10,0,0q�$	��Top=�
�������$��20(��� $��11,39,37,271q���Z� .+��� $��이름 :��$��Right=�$	��Top=�$��15(�
�������$��31��$��50��$ 40��$ 10�� J� RenderTransformOrigin$ 0.567,0.548&��� $
��ID :��$
��Left=�$	��Top=�$��15(�
�������$ 57��$ 50���� $
��PW :��$
��Left=�$	��Top=�$��15(�
�������$ 49��$ 88���� -   $ 	txtDBName��$��dbname��$	 108��$ 15��$	��170��$��26��$��15(��� -   $ txtDBID��$
��dbID��$	 108��$ 50��$	��170��$��26��$��15(��� -   $ txtDBPW��$
��dbPW��$	 109��$ 88��$	��170��$��26��$��15(��� -   $ btnDBServer��$
��Left=�$	��Top=�$��87��$��41��$	 284��$ 95��$��288,167,0,0q�#��[  ��Z� ��Z� .���� $��Resources\btn_save.png���� $��더존DB��$
��Left=�$��
12,224,0,0q�$	��Top=�
�������$��20(��� -   $ tbxTheZoneMessage��$
��Left=�$��
12,266,0,0q�$
 Wrap=�$	��Top=�$��32��$	��372��
��������� $��Server PortMIS DB��$
��Left=�$��
11,309,0,0q�$	��Top=�
�������$��20(��� -   $ tbxPortMISMessage��$
��Left=�$��
11,351,0,0q�$
 Wrap=�$	��Top=�$��32��$	��372��
��������� $��Client List�����	#FF3195D3��Z� .��� ��Z� .+��� $��Client List��$
��Left=�$��10,9,0,0q�$	��Top=�
�������$��20(�$��37��$	��120���� -    ��Name$ ListViewClient��$
��Left=�$	��Top=�$	��381��$	��374��$��	10,47,0,0q� �� View �� .P��� $��	Client ID�� �� Width$	 180�� �� DisplayMemberBinding �� *Name+�� $��State��$	 180�� �� *IsConnected+�� $	��Log�����	#FF3195D3��Z� .��� ��Z� .+��� $	��Log��$
��Left=�$��10,9,0,0q�$	��Top=�
�������$��20(�$��37��$	��120���� -   $ tbxLog��$
��Left=�$	��Top=�$	��376��$	��374��$��	10,52,0,0q�$
 Wrap=�$
��Auto=�$
��Auto=���. �� -   $
��Left=�$��
335,10,0,0q�$	��Top=�$��93��$��37����Z� ��Z� .���� $��Resources\btn_close.png��