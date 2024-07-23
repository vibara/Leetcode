void Step(int[] positions, int[] healths, string directions, int maxPosition) {
        for (int i = 0; i < positions.Length; i++) {
        if (positions[i] > 0) {
            switch (directions[i]) {
                case 'L': positions[i]--; break; // 0 = сошел с дистанции (влево) 
                case 'R':
                    positions[i]++;
                    if (positions[i] > maxPosition) {
                        // тоже сошёл, тогда делаем позицию 0
                        positions[i] = 0;
                    }
                    break;
                default: // такого быть не должно, но нужно выставить по корректной спецификации языка
                    break;
            }
        }
    }
}

bool CanStop(int[] positions, int[] healths, string directions) {
    // Условия для остановки: 
    // 1) Число выживших роботов = 1 или 0, либо
    // 2) Роботы не встретятся - у всех позиций одно значение, либо нет пар, где позиция L больше позиции R   
    int numberAlive = 0;
    int rightmostL = 0; // самый правый робот, который движется влево
    int leftmostR = positions.Length + 1; // самый левый робот, который движется вправо (его позиция)             
    for (int i = 0; i < positions.Length; i++) {
        if (positions[i] > 0) {
            numberAlive++;
            switch (directions[i]) {
                case 'R':
                    if (positions[i] < leftmostR) {
                        leftmostR = positions[i];
                    }
                    break;
                case 'L':
                    if (positions[i] > rightmostL) {
                        rightmostL = positions[i];
                    }
                    break;
                default:
                    break;
            }
        }
    }
    return
        (numberAlive < 2) ||
        (rightmostL < leftmostR);
}

// возвращает словарь, где ключ - номер позиции, значение - индекс в массиве (номер робота -1)
IDictionary<int, int> ByPositions(int[] positions) {
    var result = new Dictionary<int, int>();
    for (int i = 0; i < positions.Length; i++) {
        if (positions[i] > 0) {
            result[positions[i]] = i;
        }
    }
    return result;
}

void CheckCollisionsNextStep(int[] positions, int[] healths, string directions) {
    // проверка тех случаев, в которых роботы пересекутся в следующий ход
    // (не тех случаев, когда они уже оказались на одной позиции)
    var byPositions = ByPositions(positions);
    int position = 1;
    while (position <= byPositions.Keys.Max()) {
        if (byPositions.ContainsKey(position) && byPositions.ContainsKey(position + 1)) {
            int robotIndex = byPositions[position];
            int robotIndexNext = byPositions[position + 1];
            if (positions[robotIndex] > 0 && 
                positions[robotIndexNext] == positions[robotIndex] + 1 &&
                directions[robotIndex] == 'R' &&
                directions[robotIndexNext] == 'L') {
                // пересекутся в следующий ход. Убираем слабого, но не двигаем выжившего
                if (healths[robotIndex] == healths[robotIndexNext]) {
                    // одинаковое здоровье, убираем обоих
                    positions[robotIndex] = 0;
                    positions[robotIndexNext] = 0;
                    healths[robotIndex] = 0;
                    healths[robotIndexNext] = 0;
                }
                else if (healths[robotIndex] > healths[robotIndexNext]) {
                    healths[robotIndex]--;
                    healths[robotIndexNext] = 0;
                    positions[robotIndexNext] = 0;
                }
                else if (healths[robotIndex] < healths[robotIndexNext]) {
                    healths[robotIndex] = 0;
                    healths[robotIndexNext]--;
                    positions[robotIndex] = 0;
                }
                // три случая выше взаимоисключающие, так что больше else нет
                position++; // эту пару уже рассмотрели, проверяем на позицию правее второго в паре
            }
        }
        position++;
    }
}

void CheckCollisionsAfterStep(int[] positions, int[] healths, string directions) {
    var existingPositions = new Dictionary<int, int>();
    for (int i = 0; i < positions.Length; i++) {
        int pos = positions[i];
        if (pos > 0) {
            if (existingPositions.ContainsKey(pos)) {
                int otherRobotIndex = existingPositions[pos];
                if (healths[i] == healths[otherRobotIndex]) {
                    // убираем обоих
                    positions[i] = 0;
                    positions[otherRobotIndex] = 0;
                    healths[i] = 0;
                    healths[otherRobotIndex] = 0;
                    existingPositions.Remove(pos);
                }
                else if (healths[i] > healths[otherRobotIndex]) {
                    positions[otherRobotIndex] = 0;
                    healths[otherRobotIndex] = 0;
                    healths[i]--;
                    existingPositions[pos] = i;
                }
                else if (healths[i] < healths[otherRobotIndex]) {
                    positions[i] = 0;
                    healths[i] = 0;
                    healths[otherRobotIndex]--;
                    existingPositions[pos] = otherRobotIndex; // в принципе, не нужно, ибо уже этот индекс там есть
                }
            }
            else {
                existingPositions[pos] = i;
            }
        }
    }
}

IList<int> BuildResultList(int[] positions, int[] healths, string directions) {
    var results = new List<int>();
    for (int i = 0; i < positions.Length; i++) {
        if (healths[i] > 0) {
            results.Add(healths[i]);
        }
    }
    return results;
}



IList<int> SurvivedRobotsHealths(int[] positions, int[] healths, string directions) {
    int maxPosition = positions.Max();
    while (!CanStop(positions, healths, directions)) {
        CheckCollisionsNextStep(positions, healths, directions);
        Step(positions, healths, directions, maxPosition);
        CheckCollisionsAfterStep(positions, healths, directions);
    }
    return BuildResultList(positions, healths, directions);
}

//int[] positions = [ 1, 2, 5, 6 ];
//int[] healths = [10, 10, 11, 11];
//string directions = "RLRL";

//int[] positions = [1, 40];
//int[] healths = [10, 11];
//string directions = "RL";

int[] positions = [22, 21];
int[] healths = [397, 528];
string directions = "LR";

var result = SurvivedRobotsHealths(positions, healths, directions);
Console.WriteLine(String.Join("; ", result));