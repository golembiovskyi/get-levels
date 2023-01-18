using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BIT
{
    #region Attributes

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]

    #endregion

    class GetLevelsCommand : IExternalCommand
    {
        private const string commandName = "GetLevelsCommand";
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var doc = commandData.Application.ActiveUIDocument.Document;
                var levels = GetLevels(doc);

                // use this instead first one in case you want to get all the levels
                // even if there are no spaces in the requested document
                // basically the second one returns all the levels

                //var levels = GetLevelsAlternative(doc);

                string commandMessage = "No any levels found";

                if (levels.Count != 0)
                {
                    commandMessage = string.Join(Environment.NewLine, levels.Select(l => l.Name).ToArray());
                }

                TaskDialog.Show(commandName, commandMessage);
            }
            catch (Exception ex)
            {
                TaskDialog.Show(commandName, ex.Message);
                return Result.Failed;
            }
            return Result.Succeeded;
        }

        /// <summary>
        /// GetLevels - retrieves all the levels from the requested document by using existing spaces
        /// </summary>
        /// <param name="doc">the requested document</param>
        /// <returns>collection of levels</returns>
        private ICollection<Level> GetLevels(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(SpatialElement))
                .Cast<SpatialElement>()
                .Where(r => r.Area > 0)
                .Select(r => r.Level)
                .Cast<Level>()
                .ToHashSet(new LevelComparer());
        }

        /// <summary>
        /// GetLevelsAlternative - retrieves all the levels from the requested document directly
        /// </summary>
        /// <param name="doc">the requested document</param>
        /// <returns>collection of levels</returns>
        private ICollection<Level> GetLevelsAlternative(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .ToList();
        }
    }
}
