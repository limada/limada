namespace Limaki.Common.Text.HTML {
    using System;
    
    using System.Collections.Generic;
    /**TagStyler sammelt 
     * HTML-Dateien in Form von Tags und alle
     * Styles aus diesen Dateien, die zu einem CSS generiert werden*/
    public class TagStyler {
        /**Für CSS und style-Tags formatierte Formatierungsanweisungen*/
        private string _CSS = null;
        /**Liste der Tagger-Objekte mit den eingelesenen HTML-Dateien*/
        private List<Tagger> _tagged = new List<Tagger>();
        /**Sammlung der CSS/style-Tag-Formatierungsanweisungen*/
        private Styler styled = new Styler();
        
        public TagStyler() {}

        /**Konstruktor mit einem string = Inhalt einer HTML-Datei, die aufgenommen werden soll*/
        public TagStyler(string content) {
            setTagger(content);
        }
        /**Konstruktor mit einem string = Inhalt einer HTML-Datei, die aufgenommen werden soll, und einer Höchstanzahl an Textzeichen, bis zu denen bearbeitet werden soll*/
        public TagStyler(string content, int textLength) {
            setTagger(content, textLength);
        }

        /**Fügt den Tagger und die Styles aus einer Datei an (content = Dateiinhalt)*/
        public void setTagger(string content) {
            var tg = new Tagger(content);
            _tagged.Add(tg);
            styled.AddStyles(tg.Styles);
        }

        /**Fügt den Tagger und die Styles aus einer Datei an (content = Dateiinhalt), beendet nach der Höchstzeichenanzahl*/
        public void setTagger(string content, int textLength) {
            var tg = new Tagger(content, textLength);
            _tagged.Add(tg);
            styled.AddStyles(tg.Styles);
        }

        /**Bastelt ein einziges Style-CSS zusammen*/
        private void setCSS() {
            this._CSS = styled.CSS(true, true);
        }

        /**Gibt die Liste der Tagger-Objekte (= der eingelesenen, analysierten HTML-Dateien) zurück*/
        public List<Tagger> Taggers {
            get { return this._tagged; }
        }
        
        ///**Gibt die Liste der gesammelten styles zurück*/
        //public List<Styler> sstyled() {
        //    return this.styled;
        //}

        /**Gibt einen für die Verwendung als CSS-Datei oder style-Tag formatierten Text mit Formatierungsanweisungen zurück*/
        public string CSS {
            get {
                setCSS ();
                return this._CSS;
            }
        }
    }
}