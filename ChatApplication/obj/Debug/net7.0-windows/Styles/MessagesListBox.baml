   M S B A M L   `   `   `  ����J  FChatApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:ChatApplication.ModelsChatApplication.Models  5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   MessagesListBoxItemTemplate  � RowDefinitions �� StringFormat ��Name �� CornerRadius �� ElementName ��Name  TextBlockMessageStyle �� Triggers �� Binding �� Value(   "ChatApplication.Models.MessageType	 �� 
TargetName
 �� Property �� Value  ListBoxItemsContainerStyle �� 
TargetType �� 
TargetType �� Padding J� SnapsToDevicePixels�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .model$clr-namespace:ChatApplication.Models  5      %X  &	        &	 O    �� 5      .R�5   
   �   5      �� 5      �� 5      .+�5
      �� ���#888888$��Calibri)�6-   $��10(�6B   $��Center=�6P   $��0��6m   ��6z   �� *Date+$ dd MMMM yyyy HH:mm���� 5      $ border��6   ���#F7EFE5$ 15��6:   $��10q�6~   $��1��6�   ��6L   �� *ActualHeight+$
 Text����6�   �� *ActualWidth+$
 Text���� 5      $
 Text��6   $��1��6&   #���  63   ��6c   �� *Text+5       5   
   �� 5       6   �� *Type+ 5        5      Outgoing=�62   .a�5      �� $
	 Text��62   
 �@��TextAlignment$ Right=�6D   �� 5      $
	 Text��68   
 � ��$ Right=�6J   �� 5      $	 border��68   
 � ��$ Right=�6L   �� 5      $	 border��6/   
 � �� �#C3ACD06C   5      �� 5       6   �� *Type+ 5        5      Incoming=�62   .a�5      �� $
	 Text��62   
 �@��TextAlignment$
 Left=�6D   �� 5      $
	 Text��68   
 � ��$
 Left=�6J   �� 5      $	 border��68   
 � ��$
 Left=�6L   5      5!      �� 5"      " ��6/   .�5#   
   �� 
 � ��$ Stretch=�67   �� 5$   
   
 � �� 5%      �� 5&      " ��6"   .R�5'      �� $��_Border��6   $ 2q�6,   $
 true��68   ._�5(      �� 5)      5*      5,      5-      5.      