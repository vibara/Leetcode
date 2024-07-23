int[] nums = [3, 7, 8, 1, 1, 5];
var space = 2;

var solution = new Solution();
Console.WriteLine(solution.DestroyTargets(nums, space));

public class Solution {
    public int DestroyTargets(int[] nums, int space) {
        int minVal = int.MaxValue;
        int maxNumberDestroy = 0;
        for (int i = 0; i < nums.Length; i++) {
            int numberDestroy = 0;
            int numBase = nums[i];
            for (int forDestroyI = 0; forDestroyI < nums.Length; forDestroyI++) {
                if ((nums[forDestroyI] >= numBase) &&
                    ((nums[forDestroyI] - numBase) % space) == 0) {
                    numberDestroy++;
                    if (numberDestroy > maxNumberDestroy) {
                        minVal = numBase;
                        maxNumberDestroy = numberDestroy;
                    }
                    else if (numberDestroy == maxNumberDestroy) {
                        if (numBase < minVal) {
                            minVal = numBase;
                        }
                    }
                }
            }
        }
        return minVal;
    }
}

