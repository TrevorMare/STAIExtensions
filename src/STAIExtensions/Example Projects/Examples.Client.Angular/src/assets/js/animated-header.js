// Got this from the following code pen and made some adjustments to make it work
// https://codepen.io/AzazelN28/pen/OQvpPG

function animateHeader(title, requiredText) {
    const CHAR_TIME = 30;
    
    let text, index;
    
    function requestCharAnimation(char, value) {
      setTimeout(function() {
        char.textContent = value;
        char.classList.add("fade-in");
      }, CHAR_TIME);
    }
    
    function addChar() {
      const char = document.createElement("span");
      char.classList.add("char");
      char.textContent = "▌";
      title.appendChild(char);
      requestCharAnimation(char, text.substr(index++, 1));
      if (index < text.length) {
        requestChar();
      }
    }
    
    function requestChar(delay = 0) {
      setTimeout(addChar, CHAR_TIME + delay);
    }
    
    function start() {
      index = 0;
      text = requiredText.trim();
      title.textContent = "";
      requestChar(1000);
    }
    start();
}




