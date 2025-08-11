#include <Keyboard.h>

String inputString = "";

void setup() {
  Serial.begin(9600);
  while (!Serial); // Chờ Serial
  Keyboard.begin();
  Serial.println("Ready for full keyboard input.");
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

void handleInput(String cmd) {
  cmd.trim();
  cmd.toUpperCase();

  // Hỗ trợ combo như "Z+X+C"
  if (cmd.indexOf('+') != -1) {
    int start = 0;
    while (start < cmd.length()) {
      int idx = cmd.indexOf('+', start);
      String token = (idx == -1) ? cmd.substring(start) : cmd.substring(start, idx);
      token.trim();
      handleSingleKey(token);
      start = idx + 1;
      delay(100);
    }
  } else {
    handleSingleKey(cmd);
  }

  Serial.print("Handled: ");
  Serial.println(cmd);
}

void handleSingleKey(String cmd) {
  // Giữ phím
  if (cmd.endsWith("_DOWN")) {
    String key = cmd.substring(0, cmd.length() - 5);
    pressKey(key);
    return;
  }

  // Thả phím
  if (cmd.endsWith("_UP")) {
    String key = cmd.substring(0, cmd.length() - 3);
    releaseKey(key);
    return;
  }

  // Nhấn 1 lần
  pressAndReleaseKey(cmd);
}

void pressAndReleaseKey(String key) {
  pressKey(key);
  delay(100);
  releaseKey(key);
}

void pressKey(String key) {
  key.trim();
  if (key.length() == 1 && isPrintable(key.charAt(0))) {
    Keyboard.press(key.charAt(0));
  } else if (key == "ENTER") Keyboard.press(KEY_RETURN);
  else if (key == "ESC") Keyboard.press(KEY_ESC);
  else if (key == "TAB") Keyboard.press(KEY_TAB);
  else if (key == "SPACE") Keyboard.press(' ');
  else if (key == "LEFT") Keyboard.press(KEY_LEFT_ARROW);
  else if (key == "JUMP_LEFT"){
    Keyboard.press(KEY_LEFT_ARROW);
    delay(100);
    Keyboard.write('e'); // nhấn 1 lần e
    delay(100);
    Keyboard.release(KEY_LEFT_ARROW);
  }
  else if (key == "JUMP_LEFT_SUNG"){
    Keyboard.press(KEY_LEFT_ARROW);
    delay(100);
    for (int i = 0; i < 2; i++) {
    Keyboard.press(KEY_LEFT_ALT); // Nhấn Alt
    delay(100);                   // Giữ 100ms
    Keyboard.release(KEY_LEFT_ALT); // Thả Alt
    delay(100);                   // Nghỉ 100ms trước lần tiếp theo
    }
    delay(100);
    Keyboard.release(KEY_LEFT_ARROW);
  } 
  else if (key == "RIGHT") Keyboard.press(KEY_RIGHT_ARROW);
  else if (key == "JUMP_RIGHT"){
    Keyboard.press(KEY_RIGHT_ARROW);
    delay(100);
    Keyboard.write('e'); // nhấn 1 lần e
    delay(100);
    Keyboard.release(KEY_RIGHT_ARROW);
  }
  else if (key == "JUMP_RIGHT_SUNG"){
    Keyboard.press(KEY_RIGHT_ARROW);
    delay(100);
    for (int i = 0; i < 2; i++) {
    Keyboard.press(KEY_LEFT_ALT); // Nhấn Alt
    delay(100);                   // Giữ 100ms
    Keyboard.release(KEY_LEFT_ALT); // Thả Alt
    delay(100);                   // Nghỉ 100ms trước lần tiếp theo
    }
    delay(100);
    Keyboard.release(KEY_RIGHT_ARROW);
  }
  else if (key == "UP") Keyboard.press(KEY_UP_ARROW);
  else if (key == "DOWN") Keyboard.press(KEY_DOWN_ARROW);
  else if (key == "BACKSPACE") Keyboard.press(KEY_BACKSPACE);
  else if (key == "DELETE") Keyboard.press(KEY_DELETE);
  else if (key == "HOME") Keyboard.press(KEY_HOME);
  else if (key == "END") Keyboard.press(KEY_END);
  else if (key == "PAGEUP") Keyboard.press(KEY_PAGE_UP);
  else if (key == "PAGEDOWN") Keyboard.press(KEY_PAGE_DOWN);
  else if (key == "SHIFT") Keyboard.press(KEY_LEFT_SHIFT);
  else if (key == "CTRL") Keyboard.press(KEY_LEFT_CTRL);
  else if (key == "ALT") Keyboard.press(KEY_LEFT_ALT);
  else if (key == "GUI" || key == "WIN") Keyboard.press(KEY_LEFT_GUI);
  else if (key == "CAPSLOCK") Keyboard.press(KEY_CAPS_LOCK);
  
  else if (key.startsWith("F")) {
    int fn = key.substring(1).toInt();
    if (fn >= 1 && fn <= 12)
      Keyboard.press(KEY_F1 + (fn - 1));
  }
}

void releaseKey(String key) {
  key.trim();
  if (key.length() == 1 && isPrintable(key.charAt(0))) {
    Keyboard.release(key.charAt(0));
  } else if (key == "ENTER") Keyboard.release(KEY_RETURN);
  else if (key == "ESC") Keyboard.release(KEY_ESC);
  else if (key == "TAB") Keyboard.release(KEY_TAB);
  else if (key == "SPACE") Keyboard.release(' ');
  else if (key == "LEFT") Keyboard.release(KEY_LEFT_ARROW);
  else if (key == "JUMP_LEFT"){
     Keyboard.press(KEY_LEFT_ARROW);
    delay(100);
    Keyboard.write('e'); // nhấn 1 lần e
    delay(100);
    Keyboard.release(KEY_LEFT_ARROW);
  }
  else if (key == "JUMP_LEFT_SUNG"){
    Keyboard.press(KEY_LEFT_ARROW);
    delay(100);
    for (int i = 0; i < 2; i++) {
    Keyboard.press(KEY_LEFT_ALT); // Nhấn Alt
    delay(100);                   // Giữ 100ms
    Keyboard.release(KEY_LEFT_ALT); // Thả Alt
    delay(100);                   // Nghỉ 100ms trước lần tiếp theo
    }
    delay(100);
    Keyboard.release(KEY_LEFT_ARROW);
  }
  else if (key == "RIGHT") Keyboard.release(KEY_RIGHT_ARROW);
  else if (key == "JUMP_RIGHT"){
    Keyboard.press(KEY_RIGHT_ARROW);
    delay(100);
    Keyboard.write('e'); // nhấn 1 lần e
    delay(100);
    Keyboard.release(KEY_RIGHT_ARROW);
  }
  else if (key == "JUMP_RIGHT_SUNG"){
    Keyboard.press(KEY_RIGHT_ARROW);
    delay(100);
     for (int i = 0; i < 2; i++) {
    Keyboard.press(KEY_LEFT_ALT); // Nhấn Alt
    delay(100);                   // Giữ 100ms
    Keyboard.release(KEY_LEFT_ALT); // Thả Alt
    delay(100);                   // Nghỉ 100ms trước lần tiếp theo
    }
    delay(100);
    Keyboard.release(KEY_RIGHT_ARROW);
  }
  else if (key == "UP") Keyboard.release(KEY_UP_ARROW);
  else if (key == "DOWN") Keyboard.release(KEY_DOWN_ARROW);
  else if (key == "BACKSPACE") Keyboard.release(KEY_BACKSPACE);
  else if (key == "DELETE") Keyboard.release(KEY_DELETE);
  else if (key == "HOME") Keyboard.release(KEY_HOME);
  else if (key == "END") Keyboard.release(KEY_END);
  else if (key == "PAGEUP") Keyboard.release(KEY_PAGE_UP);
  else if (key == "PAGEDOWN") Keyboard.release(KEY_PAGE_DOWN);
  else if (key == "SHIFT") Keyboard.release(KEY_LEFT_SHIFT);
  else if (key == "CTRL") Keyboard.release(KEY_LEFT_CTRL);
  else if (key == "ALT") Keyboard.release(KEY_LEFT_ALT);
  else if (key == "GUI" || key == "WIN") Keyboard.release(KEY_LEFT_GUI);
  else if (key == "CAPSLOCK") Keyboard.release(KEY_CAPS_LOCK);
  else if (key.startsWith("F")) {
    int fn = key.substring(1).toInt();
    if (fn >= 1 && fn <= 12)
      Keyboard.release(KEY_F1 + (fn - 1));
  }
}
