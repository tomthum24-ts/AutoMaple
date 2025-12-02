#include <Keyboard.h>

String inputString = "";

void setup() {
  Serial.begin(9600);
  while (!Serial);
  Keyboard.begin();
  Serial.println("Ready.");
}

void loop() {
  while (Serial.available()) {
    char c = Serial.read();
    if (c == '\n') {
      handleInput(inputString);
      inputString = "";
    } else {
      inputString += c;
    }
  }
}

// ==========================
// HANDLE INPUT
// ==========================

void handleInput(String cmd) {
  cmd.trim();
  cmd.toUpperCase();

  handleSingleKey(cmd);

  Serial.print("OK: ");
  Serial.println(cmd);
}

void handleSingleKey(String cmd) {
  
  // ======================
  // MAPLE JUMP COMMANDS
  // ======================

  if (cmd == "JUMP_LEFT")      { jumpWithKey(KEY_LEFT_ARROW);  return; }
  if (cmd == "JUMP_RIGHT")     { jumpWithKey(KEY_RIGHT_ARROW); return; }
  if (cmd == "JUMP_UP")        { jumpWithKey(KEY_UP_ARROW);    return; }
  if (cmd == "JUMP_DOWN")      { jumpWithKey(KEY_DOWN_ARROW);  return; }

  // ======================
  // NORMAL SINGLE KEY
  // ======================

  if (cmd.length() == 1 && isPrintable(cmd.charAt(0))) {
    Keyboard.press(cmd.charAt(0));
    delay(30);
    Keyboard.release(cmd.charAt(0));
    return;
  }

  if (cmd == "LEFT")  { pressQuick(KEY_LEFT_ARROW); return; }
  if (cmd == "RIGHT") { pressQuick(KEY_RIGHT_ARROW); return; }
  if (cmd == "UP")    { pressQuick(KEY_UP_ARROW); return; }
  if (cmd == "DOWN")  { pressQuick(KEY_DOWN_ARROW); return; }

  if (cmd == "SPACE") { pressQuick(' '); return; }
  if (cmd == "ENTER") { pressQuick(KEY_RETURN); return; }
  if (cmd == "ESC")   { pressQuick(KEY_ESC); return; }
  if (cmd == "TAB")   { pressQuick(KEY_TAB); return; }

  // F-keys
  if (cmd.startsWith("F")) {
    int f = cmd.substring(1).toInt();
    if (f >= 1 && f <= 12) {
      pressQuick(KEY_F1 + (f - 1));
    }
    return;
  }
}

// ==========================
// JUMP FUNCTION (MAPLE)
// Arrow + E
// ==========================

void jumpWithKey(uint8_t arrowKey) {

  Keyboard.press(arrowKey);
  delay(50);

  Keyboard.press('e');
  delay(50);
  Keyboard.release('e');

  delay(20);
  Keyboard.release(arrowKey);
}

// ==========================
// QUICK PRESS
// ==========================

void pressQuick(uint8_t k) {
  Keyboard.press(k);
  delay(30);
  Keyboard.release(k);
}
