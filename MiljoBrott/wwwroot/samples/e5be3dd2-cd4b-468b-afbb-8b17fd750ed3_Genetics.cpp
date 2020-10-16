//---------------------------------------------------------------------
//---------------------------------------------------------------------
//---------------------------------------------------------------------
//  Class Template - Assignment 2.2
//  Artificial Intelligence for Game Programming I - 5SD809
//  Uppsala University
//---------------------------------------------------------------------
//---------------------------------------------------------------------
//---------------------------------------------------------------------
#include "Genetics.h"
//---------------------------------------------------------------------
//---------------------------------------------------------------------
Genetics::Genetics(){
    //--------------------------------------------
    mChromIdx2GoalFunc = new double[MAX_CHROMS_M];
    mCityCoordinates = new double[MAX_CITIES_N][2];
    mGP = new int[MAX_CHROMS_M][MAX_CITIES_N];
    //--------------------------------------------
    mW = 1024;//1280;
    mH = int(double(mW)*10./16.);
    sRandSeed = 0;
    //--------------------------------------------
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------



//---------------------------------------------------------------------
//---------------------------------------------------------------------
//                                 GFX
//---------------------------------------------------------------------
//---------------------------------------------------------------------
void Genetics::Init(){
    InitFirstGen();
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
void Genetics::Draw(){
    //--------------------------------------------
    For (i,GFX_GENS_PER_FRAME) EvalNextGen();
    //--------------------------------------------
    printf("Generation: %d ... ",mGenIdx);
    double x = GoalFunc(0);
    printf("Best Path:  %3.1lf (%3.1lf%%)\n",
           x,100.*x/mStartValueBestChrom);
    //--------------------------------------------
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
void Genetics::MainLoop(){
    Init();
    For (i,MAX_NEW_GENS) Draw();
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------



//---------------------------------------------------------------------
//---------------------------------------------------------------------
//                                Rand
//---------------------------------------------------------------------
//---------------------------------------------------------------------
int Genetics::Rand(int a){
    if (a < 1) a = 1; else if (a > 32768) a = 32768;
    sRandSeed = sRandSeed * 1103515245 + 12345;
    return (unsigned int)(sRandSeed/65536) % a;//return {0,1,...,a-1}
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
double Genetics::RandX(){
    return double(Rand(32768))/32768.;//return [0,1[
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------



//---------------------------------------------------------------------
//---------------------------------------------------------------------
//                            Goal Function
//---------------------------------------------------------------------
//---------------------------------------------------------------------
double Genetics::GoalFunc(int chromIdx){
    double dist = 0., x0, x1, dx, y0, y1, dy;
    For (i,mN){
        int from = i;
        int to = i+1;
        if (to == mN) to = 0;
        int cityFromIdx = mGP[chromIdx][from];
        int cityToIdx = mGP[chromIdx][to];
        x0 = mCityCoordinates[cityFromIdx][0];
        x1 = mCityCoordinates[cityToIdx][0];
        dx = x1 - x0;
        y0 = mCityCoordinates[cityFromIdx][1];
        y1 = mCityCoordinates[cityToIdx][1];
        dy = y1 - y0;
        dist += sqrt(dx*dx + dy*dy);
    }
    return dist;
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
inline void Genetics::EvalGoalFuncs(){
    For (m,mM) mChromIdx2GoalFunc[m] = GoalFunc(m);
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------



//---------------------------------------------------------------------
//---------------------------------------------------------------------
//                            Fundamentals
//---------------------------------------------------------------------
//---------------------------------------------------------------------
inline void Genetics::CopyChroms(int fromChromIdx, int toChromIdx){
    For (i,mN) mGP[toChromIdx][i] = mGP[fromChromIdx][i];
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
void Genetics::SwapChroms(int cIdx1, int cIdx2){
    double tmp = mChromIdx2GoalFunc[cIdx1];
    mChromIdx2GoalFunc[cIdx1] = mChromIdx2GoalFunc[cIdx2];
    mChromIdx2GoalFunc[cIdx2] = tmp;
    For (j,mN-1){
        int tmp = mGP[cIdx1][j];
        mGP[cIdx1][j] = mGP[cIdx2][j];
        mGP[cIdx2][j] = tmp;
    }
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
inline bool Genetics::IsEqual(int cIdx1, int cIdx2){
    For (n,mN) if (mGP[cIdx1][n] != mGP[cIdx2][n]) return false;
    return true;
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
void Genetics::PartialSort(){
    //--------------------------------------------
    //--------------------------------------------
    //   Mark all Children Identical to Parents
    //--------------------------------------------
    //--------------------------------------------
    bool skipChrom[MAX_CHROMS_M];
    For (i,mM) skipChrom[i] = false;
    For (i,PARENTS_N){
        for (int ii = PARENTS_N; ii < mM; ii++){
            if ((mChromIdx2GoalFunc[i] == mChromIdx2GoalFunc[ii]) &&
                IsEqual(i,ii))
                skipChrom[ii] = true;
        }
    }
    //--------------------------------------------
    //--------------------------------------------
    //        Mark Duplicates of Children
    //--------------------------------------------
    //--------------------------------------------
    for (int i = PARENTS_N; i < mM; i++){
        for (int ii = i+1; ii < mM; ii++){
            if ((mChromIdx2GoalFunc[i] == mChromIdx2GoalFunc[ii]) &&
                IsEqual(i,ii))
                skipChrom[ii] = true;
        }
    }
    //--------------------------------------------
    //--------------------------------------------
    //              Select Survivals
    //--------------------------------------------
    //--------------------------------------------
    double min;
    int    minIdx;
    For (i,PARENTS_N){
        //----------------------------------------
        //----------------------------------------
        min = mChromIdx2GoalFunc[i];
        minIdx = i;
        for (int ii = i+1; ii < mM; ii++){
            if (skipChrom[ii]) continue;
            if (mChromIdx2GoalFunc[ii] < min){
                min = mChromIdx2GoalFunc[ii];
                minIdx = ii;
            }
        }
        if (minIdx != i) SwapChroms(i,minIdx);
        //----------------------------------------
        //----------------------------------------
    }
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
void Genetics::CloneChroms(){
    for (int toIdx = PARENTS_N; toIdx < mM; toIdx++){
        int fromIdx = toIdx % PARENTS_N;
        CopyChroms(fromIdx,toIdx);
    }  
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------



//---------------------------------------------------------------------
//---------------------------------------------------------------------
//                              Basic Ops
//---------------------------------------------------------------------
//---------------------------------------------------------------------
void Genetics::Mutate(int chromIdx){//Cities {1,2,...,mN-2}
    int a = Rand(mN-1), b = Rand(mN-1);
    while (a == b) b = Rand(mN-1);
    Mutate(chromIdx,a,b);
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
inline void Genetics::Mutate(int chromIdx, int a, int b){
    int tmp = mGP[chromIdx][a];
    mGP[chromIdx][a] = mGP[chromIdx][b];
    mGP[chromIdx][b] = tmp;
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
void Genetics::TransformGP(){
    //--------------------------------------------
    //--------------------------------------------
    for (int i = PARENTS_N; i < mM; i++){
        //----------------------------------------
        //----------------------------------------
        if (i >= mM-2){
            if (i == mM-2){MPC(i,i+1); i++; break;}
            else {Mutate(i); break;}
        }
        //----------------------------------------
        //----------------------------------------
        int rnd = Rand(MPCS_PER_MUTATION+1);//0-3
        if (rnd == 0) Mutate(i);
        else {MPC(i,i+1); i++;}
        //----------------------------------------
        //----------------------------------------
    }
    //--------------------------------------------
    //--------------------------------------------
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------



//---------------------------------------------------------------------
//---------------------------------------------------------------------
//                             City Coords
//---------------------------------------------------------------------
//---------------------------------------------------------------------
void Genetics::GenCityCoords(){
    //--------------------------------------------
    //--------------------------------------------
    double min[2], delta[2];
    min[0] = .1 * double(mW); delta[0] = .8 * double(mW);
    min[1] = .1 * double(mH); delta[1] = .8 * double(mH);
    For (i,mN) For (j,2){
        mCityCoordinates[i][j] = min[j] + delta[j] * RandX();}
    //--------------------------------------------
    //--------------------------------------------
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
//                      Generate First Generation
//---------------------------------------------------------------------
//---------------------------------------------------------------------
void Genetics::InitFirstGen(){
    //--------------------------------------------
    //--------------------------------------------
    sRandSeed = (unsigned int)time(NULL);//Random seed for each run
    GenCityCoords();
    For (m,mM){
        For (n,mN) mGP[m][n] = n;
        For (i,1000) Mutate(m);
    }
    EvalGoalFuncs();
    mGenIdx = 0;
    //--------------------------------------------
    //--------------------------------------------
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
//                      Generate Next Generation
//---------------------------------------------------------------------
//---------------------------------------------------------------------
void Genetics::EvalNextGen(){
    //--------------------------------------------
    PartialSort();
    if (mGenIdx == 0) mStartValueBestChrom = mChromIdx2GoalFunc[0];
    //--------------------------------------------
    if (mGenIdx >= MAX_NEW_GENS) return;
    //--------------------------------------------
    CloneChroms();
    TransformGP();
    EvalGoalFuncs();
    mGenIdx++;
    //--------------------------------------------
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------



//---------------------------------------------------------------------
//---------------------------------------------------------------------
//                                 MPC
//---------------------------------------------------------------------
//---------------------------------------------------------------------
void Genetics::MPC(int chromIdx1, int chromIdx2){
    //--------------------------------------------
    //----------------------------------------------
    //------------------------------------------------
    //--------------------------------------------------
    bool duplicates = false;
    /*for (int i = 0; i <= MAX_CITIES_N; i++) {//bugged if duplicate in i before the mutation window
        for (int j = i+1; j < MAX_CITIES_N; j++) {
                if (mGP[chromIdx1][i] == mGP[chromIdx1][j]) {
                    duplicates = true;
                }

        }
    }
    if (duplicates) {

        printf("There were duplicates\n");
        printf("Contents of a chromosome\n [");
        for (int i = 0; i < MAX_CITIES_N - 1; i++) {

            printf("%d, ", mGP[chromIdx1][i]);
        }
        printf("%d]", mGP[chromIdx1][MAX_CITIES_N - 1]);

        printf("\n");
    }*/
    //return;//Comment out this line for the completion of MPC_T_RepairChrom!
    //--------------------------------------------------
    //------------------------------------------------
    //----------------------------------------------
    //--------------------------------------------
    // Evaluate the MPC interval for this MPC as
    // a function of random numbers 
    //--------------------------------------------
    //--------------------------------------------
    int maxMPCWidth = mN/2;
    mWidth = 3 + Rand(maxMPCWidth-3);
    mFromIdx = Rand(mN - mWidth);
    mToIdx = mFromIdx + mWidth - 1;
    //--------------------------------------------
    //--------------------------------------------
    // Swap the data within the interval
    // [mFromIdx,mToIdx] between the chromosomes
    // chromIdx1 and chromIdx2
    //--------------------------------------------
    //--------------------------------------------
    for (int i = mFromIdx; i <= mToIdx; i++){
        int tmp = mGP[chromIdx2][i];
        mGP[chromIdx2][i] = mGP[chromIdx1][i];
        mGP[chromIdx1][i] = tmp;
    }
    //--------------------------------------------
    //--------------------------------------------
    //           Repair the chromosomes
    //--------------------------------------------
    //--------------------------------------------
    MPC_T_RepairChrom(chromIdx1);
    MPC_T_RepairChrom(chromIdx2);

    for (int i = 0; i <= MAX_CITIES_N; i++) {//bugged if duplicate in i before the mutation window
        for (int j = i + 1; j < MAX_CITIES_N; j++) {
            if (mGP[chromIdx2][i] == mGP[chromIdx2][j]) {
                duplicates = true;
            }

        }
    }
    if (false) {

        printf("Repaired list contents\n");
        printf(" [");
        for (int i = 0; i < MAX_CITIES_N - 1; i++) {

            printf("%d, ", mGP[chromIdx2][i]);
        }
        printf("%d]", mGP[chromIdx2][MAX_CITIES_N - 1]);

        printf("\n");
    }
    //--------------------------------------------
    //--------------------------------------------
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------



//---------------------------------------------------------------------
//---------------------------------------------------------------------
//                           Assignment 2.2
//---------------------------------------------------------------------
//---------------------------------------------------------------------
void Genetics::MPC_T_RepairChrom(int chromIdx){
    //--------------------------------------------
    //--------------------------------------------
    // Assignment 2.2: Complete this method and
    // upload Genetics.cpp to
    // "A2.2 - Genetic Algorithms"
    //--------------------------------------------
    //--------------------------------------------
    // Step 1: Outside of the crossover interval
    // [mFromIdx,mToIdx], replace each duplicate
    // number with -1 and add each position to the
    // list duplicatesList, followed by
    // numberOfDuplicatesN++ after each addition
    // to keep a track the number of found
    // duplicates
    //--------------------------------------------
    //--------------------------------------------
    int duplicatesList[MAX_CITIES_N];
    int numberOfDuplicatesN = 0;
    //... implement code (A)
    /*
    for (int i = 0; i < MAX_CITIES_N; i++) {//bugged if duplicate in i before the mutation window
        for (int j = i+1; j < MAX_CITIES_N; j++) {
            if (!(j >= mFromIdx && j <= mToIdx)) {
                if (mGP[chromIdx][i] == mGP[chromIdx][j]) {
                    duplicatesList[numberOfDuplicatesN] = j;
                    numberOfDuplicatesN++;
                    mGP[chromIdx][j] = -1;
                }
            }
            
        }
    }*/

    for (int i = mFromIdx; i <= mToIdx; i++) {
        for (int j = 0; j < MAX_CITIES_N; j++) {
            if (!(j >= mFromIdx && j <= mToIdx)) {
                if (mGP[chromIdx][i] == mGP[chromIdx][j]) {
                    duplicatesList[numberOfDuplicatesN] = j;
                    numberOfDuplicatesN++;
                    mGP[chromIdx][j] = -1;
                }
            }

        }
    }
    //--------------------------------------------
    //--------------------------------------------
    // Step 2: Find out which numbers that are
    // present in the chrom except for -1
    //--------------------------------------------
    //--------------------------------------------
    bool numbersIncluded[MAX_CITIES_N];
    For (i,MAX_CITIES_N) numbersIncluded[i] = false;
    //... implement code (B)
    for (int i = 0; i < MAX_CITIES_N; i++) {
        if (mGP[chromIdx][i] != -1) {
            numbersIncluded[mGP[chromIdx][i]] = true;
        }
    }
    //--------------------------------------------
    //--------------------------------------------
    // Step 3: Find out which numbers that are
    // missing in the chrom based on
    // numbersIncluded, and add each number to the
    // list missingNumbersList, followed by
    // numberOfMissingNumbersN++ after each
    // addition to keep track
    //--------------------------------------------
    //--------------------------------------------
    int missingNumbersList[MAX_CITIES_N];
    int numberOfMissingNumbersN = 0;
    //... implement code (C)
    for (int i = 0; i < MAX_CITIES_N; i++) {
        if (numbersIncluded[i] == false) {
            missingNumbersList[numberOfMissingNumbersN] = i;
            numberOfMissingNumbersN++;
        }
    }
    //--------------------------------------------
    //--------------------------------------------
    // Note: numberOfDuplicatesN !=
    // numberOfMissingNumbersN => Error!
    //--------------------------------------------
    //--------------------------------------------
    // Step 4: Fill the holes with the missing
    // numbers by ascending order (important for
    // the test to pass)
    //--------------------------------------------
    //--------------------------------------------
    //... implement code (D)
    /*
    printf("Contents of a missing Numbers\n [");
    for (int i = 0; i < numberOfMissingNumbersN - 1; i++) {

        printf("%d, ", missingNumbersList[i]);
    }
    if(numberOfMissingNumbersN > 1)
        printf("%d]", missingNumbersList[numberOfMissingNumbersN - 1]);

    printf("\n");*/
    /*for (int i = 0; i < numberOfMissingNumbersN; i++) {
        mGP[chromIdx][duplicatesList[i]] = missingNumbersList[i];
    }*/

    int nextMissingNumberIndex = 0;

    for (int j = 0; j < MAX_CITIES_N; j++) {
        if (nextMissingNumberIndex < numberOfMissingNumbersN) {
            if (mGP[chromIdx][j] == -1) {
                mGP[chromIdx][j] = missingNumbersList[nextMissingNumberIndex];
                nextMissingNumberIndex++;
            }
        }
    }
    //--------------------------------------------
    //--------------------------------------------
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
