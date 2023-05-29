window.addEventListener("message", function (event) {   
  if(event.data.action == "start") {
    $(".form-container").css({"display":"block"});
  }else if(event.data.action == "menu") {
    $(".form-container2").css({"display":"block"});
  }
});

document.getElementById("submitButton").addEventListener("click", function(event) {
  event.preventDefault();

  let Model = document.getElementById("data1").value; 
  let Cont = parseInt(document.getElementById("data2").value);
  if(isNaN(Cont)) {
    Cont = 4; 
  } else {
    if(Cont < 0 && Cont > 20) {
      Cont = 4;
    }
  }
  let armed = document.getElementById("check1").checked;
  let combat = document.getElementById("check2").checked;
  let weapon = document.getElementById("data3").value;

  $(".form-container").css({"display":"none"});
  $.post("https://c_follow/c_spawn", JSON.stringify({
      model: Model,
      cont: Cont,
      armed: armed,
      combat: combat,
      weapon: weapon
  }));
});

document.getElementById("submitButton2").addEventListener("click", function(event) {
  event.preventDefault();
  $(".form-container").css({"display":"none"});
  $(".form-container2").css({"display":"none"});
  $.post("https://c_follow/c_cancel", JSON.stringify({}));
});

document.getElementById("submitButton3").addEventListener("click", function(event) {
  event.preventDefault();
  
  let dist = document.getElementById("data4").value; 
  let anim = document.getElementById("data5").value; 

  $(".form-container2").css({"display":"none"});
  $.post("https://c_follow/c_anim", JSON.stringify({
      dist: dist,
      anim: anim
  }));
});

document.getElementById("submitButton4").addEventListener("click", function(event) {
  event.preventDefault();
  
  $(".form-container2").css({"display":"none"});
  $.post("https://c_follow/c_delete", JSON.stringify({}));
});

document.getElementById("submitButton5").addEventListener("click", function(event) {
  event.preventDefault();
  
  $(".form-container2").css({"display":"none"});
  $.post("https://c_follow/c_kill", JSON.stringify({}));
});

document.getElementById("submitButton6").addEventListener("click", function(event) {
  event.preventDefault();
  
  $(".form-container2").css({"display":"none"});
  $.post("https://c_follow/c_follow", JSON.stringify({}));
});