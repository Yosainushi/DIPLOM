<?php

if(isset($_POST['sthis'])){
  $sthis = $_POST['sthis'];
  if(empty($sthis)) { echo 'put something in this box'; }
  else echo 'ready';
}

?>