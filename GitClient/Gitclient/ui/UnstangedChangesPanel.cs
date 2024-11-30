using Gitclient.model;
using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class UnstangedChangesPanel
    {
        private int startIndex;
        private int endIndex;
        private int currentIndex;

        private UnstagedChangesService unstangedChangesService;

        public UnstangedChangesPanel(UnstagedChangesService unstangedChangesService)
        {
            this.unstangedChangesService = unstangedChangesService;
            this.startIndex = 0;
            this.currentIndex = GetCurrentIndex();
            this.endIndex = GetCurrentEndIndex();
        }

        public void Show()
        {
            Refresh();
        }

        private int GetCurrentStartIndex()
        {
            // sa returneze in fucntie de consola
            return 0;
        }

        private int GetCurrentIndex()
        {
            return 0;
        }

        private int GetCurrentEndIndex()
        {
            // calculeaza in functie de dimensiune
            return 10;
        }

        private void Refresh()
        {
            // de apelat cand se schimba ceva in ce trebuie afisat
            List<UnstagedChange> unstagedChanges = unstangedChangesService.GetCurrentUnstagedChanges(startIndex, endIndex);

            DrawPanel(unstagedChanges);
        }

        private void Navigate()
        {
            // update current index end index start index
            // daca current index == end index +1 sau current index == start index-1
            // verifica sa nu treci de 0 in jos si la fel pentru end index


            Refresh();
        }

        //private void Draw(List<UnstagedChanges> unstagedChanges)
        //{
        //    // creeaza chenarul de unstanged changes cu lista asta de chages
        //}

        private void DrawPanel(List<UnstagedChange>  unstagedChanges)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            
            for (int i = dimensions.tabHeight + 2; i < dimensions.height / 2 + 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(dimensions.width / 2 - 1, i);
                Console.Write("║");
            }

            for (int i = 1; i < dimensions.width / 2 - 1; i++)
            {
                Console.SetCursorPosition(i, dimensions.tabHeight + 1);
                Console.Write("─");
                Console.SetCursorPosition(i, dimensions.changesPanelHeight + 1);
                Console.Write("─");
            }

            Console.SetCursorPosition(0, dimensions.tabHeight + 1);
            Console.Write("┌");
            Console.SetCursorPosition(0, dimensions.height / 2 + 1);
            Console.Write("└");
            Console.SetCursorPosition(dimensions.width / 2 - 1, dimensions.tabHeight + 1);
            Console.Write("┐");
            Console.SetCursorPosition(dimensions.width / 2 - 1, dimensions.height / 2 + 1);
            Console.Write("┘");

            string unstaged = GetStatusFiles.SetStatusTabTextLength("Unstaged Changes: ");
            Console.SetCursorPosition(1, dimensions.tabHeight + 1);
            Console.Write(unstaged);

            foreach(var c in unstagedChanges)
            {
                Console.WriteLine(c.toDisplay());
            }
        }
    }
}
