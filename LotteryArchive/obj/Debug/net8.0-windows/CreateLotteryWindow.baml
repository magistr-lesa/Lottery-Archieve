   M S B A M L   `   `   `  ����  LotteryArchive.    (LotteryArchive.Views.CreateLotteryWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     6<   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     6~     � Title$.  (Создать новую лотерею��6�   $	��350��6�   $	��400��6�    � WindowStartupLocation$ CenterOwner=�6�   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5    
   �� $��0��6   $��0��6   $��Название:��6-   $��Center=�6A   $��0,0,5,5q�6\   �� 5"   
   -   5         ��Name$ txtName��5"      $��0��6#   $��1��60   $��0,0,0,5q�6@   �� 5&   
   $��1��6   $��0��6   $ ��Всего билетов:��6-   $��Center=�6F   $��0,0,5,5q�6a   �� 5(   
   -   5        $ txtTotalTickets��5(      $��1��6+   $��1��68   $��0,0,0,5q�6H   �� 5,   
   $��2��6   $��0��6   $ ��Призовой фонд:��6-   $��Center=�6F   $��0,0,5,5q�6a   �� 5.   
   -   5        $ txtPrizeFund��5.      $��2��6(   $��1��65   $��0,0,0,5q�6E   �� 52   
   $��3��6   $��0��6   $*��$Выбрать участников:��6-   $	��Top=�6K   $��0,0,5,5q�6c   �� 54   
   -   5         ��Name$ lstParticipants��54      $��3��6+   $��1��68   $	��120��6H    �� SelectionMode$ Extended=�6U    �� DisplayMemberPath$ FullName��6n   �� 58   
   $��4��6   $��2��6"    �� Orientation$ 
Horizontal=�66   $��Right=�6O   $��0,10,0,0q�6k   .+�5:      �� -   5        	 ��Name$	 btnOk��5:      $
��ОК��6$   $��75��61   $��0,0,5,0q�6<   6M   �� 5<      -   5        $��Отмена��5<      $��75��6&   61   5>      5?      5@      