   M S B A M L   `   `   `  ����  LotteryArchive!    LotteryArchive.GraphsWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     6/   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     6q     � Title$G  AГрафики потраченного и выигранного��6�   $	��500��6�   $	��800��6�    � WindowStartupLocation$ CenterOwner=�6�   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   .+�5   
   �� $<��6Графики: потрачено vs выиграно��6   $��16(�6:   $
��Bold"�6H   $��Center=�6Z   $��0,0,0,10q�6w   � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   .+�5      �� $��Потрачено��6   $��5q�6*   $��0��65   .��5      �� $
��Auto=�6   .��5!      �� -   5         ��Name$ canvasSpent��5!      $	��350��62   
�������6?   5#      5$      �� 5'      $��Выиграно��6   $��5q�6)   $��1��64   .��5)      �� $
��Auto=�6   .��5+      �� -   5        $ 	canvasWon��5+      $	��350��60   
�������6=   5-      5.      5/      50      51      